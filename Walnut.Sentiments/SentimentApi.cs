using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using Walnut.Sentiments.Models;

namespace Walnut.Sentiments
{
    public class Payload
    {
        public Payload()
        {
            Documents = new List<RequestDocument>();
        }
        public List<RequestDocument> Documents { get; set; }
    }

    public class ResponsePayload
    {
        public ResponsePayload()
        {
            Documents = new List<ResponseDocument>();
        }
        public List<ResponseDocument> Documents { get; set; }
    }

    public class RequestDocument
    {
        public string Id { get; set; }
        public string Language { get; set; }
        public string Text { get; set; }
    }
    public class ResponseDocument
    {
        public long Id { get; set; }
        public decimal Score { get; set; }
    }

    public enum Sentiment
    {
        Negative,
        Neutral,
        Positive
    }

    public class SentimentApi
    {
        private readonly static string _consumerKey = ConfigurationManager.AppSettings.Get("TextApiKey");
        private readonly static string _url = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";

        public event BatchProcessedHandler BatchProcessedEvent;
        public delegate void BatchProcessedHandler(SentimentApi s, BatchProcessedEventArgs e);

        private int _batchSize = 25;
        private List<Tweet> _tweets = new List<Tweet>();

        public SentimentApi(TwitterStreamClient stream)
        {
            stream.TweetReceivedEvent += (sender, args) =>
            {
                if (_tweets.Count < _batchSize)
                    _tweets.Add(args.Tweet);
                else
                {
                    GetSentiments(new List<Tweet>(_tweets));
                    _tweets.Clear();
                }
            };
        }

        public void GetSentiments(List<Tweet> batch)
        {
            var payload = new Payload();
            payload.Documents = batch.Select(t => new RequestDocument { Id = t.Id.ToString(), Text = t.Text, Language = "en" }).ToList();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(_url);
            httpWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", _consumerKey);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var resultObject = JsonConvert.DeserializeObject<ResponsePayload>(result,
                                        new JsonSerializerSettings()
                                        {
                                            NullValueHandling = NullValueHandling.Ignore,
                                            StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                                            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                                        });
                    // map sentiments to tweets
                    foreach (var item in batch)
                    {
                        foreach (var r in resultObject.Documents)
                        {
                            if(item.Id == r.Id)
                            {
                                item.SentimentScore = r.Score;
                                break;
                            }
                        }
                    }

                    // raise an event once its done
                    Raise(BatchProcessedEvent, new BatchProcessedEventArgs(batch));
                }
            }

        }

        public void Raise(BatchProcessedHandler handler, BatchProcessedEventArgs e)
        {
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}