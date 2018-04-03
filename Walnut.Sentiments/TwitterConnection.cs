using Microsoft.AspNet.SignalR;
using System.Configuration;

namespace Walnut.Sentiments
{
    public class TwitterConnection
    {
        // Consumer Keys + Secrets
        private readonly string _consumerKey = ConfigurationManager.AppSettings.Get("consumerKey");
        private readonly string _consumerSecret = ConfigurationManager.AppSettings.Get("consumerSecret");
        // Twitter OAuth Credentials
        private readonly string _accessKey = ConfigurationManager.AppSettings.Get("accessToken");
        private readonly string _accessToken = ConfigurationManager.AppSettings.Get("accessTokenSecret");

        private readonly IHubContext _context;

        public TwitterConnection()
        {
            _context = GlobalHost.ConnectionManager.GetHubContext<TwitterHub>();
            var config = new StreamConfig()
            {
                ConsumerKey = _consumerKey,
                ConsumerSecret = _consumerSecret,
                AccessToken = _accessKey,
                AccessSecret = _accessToken,
            };
            var stream = new TwitterStreamClient(config);

            // Subscribe to the tweet received event
            stream.TweetReceivedEvent += (sender, args) =>
            {
                // Broadcast the tweet to the client-side
                _context.Clients.All.broadcast(args.Tweet);
            };

            var sentimentAnalyser = new SentimentApi(stream);
            sentimentAnalyser.BatchProcessedEvent += (sender, args) =>
            {
                _context.Clients.All.processedBatch(args.Tweets);
            };

            stream.Start();

           

        }
    }
}