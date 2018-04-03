using System;
using System.Collections.Generic;
using Walnut.Sentiments.Models;

namespace Walnut.Sentiments
{
    public class BatchProcessedEventArgs : EventArgs
    {
        public BatchProcessedEventArgs(List<Tweet> tweets)
        {
            Tweets = tweets;
        }

        public List<Tweet> Tweets { get; private set; }
    }
}