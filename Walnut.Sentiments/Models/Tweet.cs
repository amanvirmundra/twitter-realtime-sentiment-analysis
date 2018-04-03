using Newtonsoft.Json;
using System;

namespace Walnut.Sentiments.Models
{
    public class Tweet
    {
        public Tweet()
        {
            User = new User();
        }

        [JsonProperty("id_str")]
        public long Id;

        [JsonProperty("text")]
        public string Text;

        [JsonProperty("created_at")]
        public string CreatedAt;

        [JsonProperty("retweet_count")]
        public int RetweetCount;

        public string UserName { get { return User.UserName; } }

        public User User { get; set; }

        public decimal SentimentScore { get; set; }
        public int SentimentPercentage { get { return Convert.ToInt16(Math.Round(SentimentScore, 2) * 100); } }
        public Sentiment Sentiment {
            get
            {
                if (SentimentPercentage > 70)
                    return Sentiment.Positive;
                else if (SentimentPercentage > 30)
                    return Sentiment.Neutral;
                else
                    return Sentiment.Negative;
            }
        }
    }

    public class User
    {
        [JsonProperty("time_zone")]
        public string TimeZone;

        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl;

        [JsonProperty("screen_name")]
        public string UserName;
    }
}