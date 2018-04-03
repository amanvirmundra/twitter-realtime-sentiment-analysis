using System;
using Walnut.Sentiments.Models;

namespace Walnut.Sentiments
{
    public class TweetEventArgs : EventArgs
    {
        public TweetEventArgs(Tweet tweet)
        {
            Tweet = tweet;
        }

        public Tweet Tweet { get; private set; }
    }
}