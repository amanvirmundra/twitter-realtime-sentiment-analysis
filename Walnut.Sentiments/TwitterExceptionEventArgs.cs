using System;
using System.Net;

namespace Walnut.Sentiments
{
    public class TwitterExceptionEventArgs : EventArgs
    {
        public TwitterExceptionEventArgs(TwitterException ex)
        {
            TwitterException = ex;
        }

        public TwitterException TwitterException { get; private set; }
    }

    public class TwitterException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ResponseMessage { get; set; }

        public TwitterException(HttpStatusCode code, string message)
        {
            StatusCode = code;
            ResponseMessage = message;
        }
        public override string ToString()
        {
            return string.Format("Twitter WebException: {0} ({1})", StatusCode, ResponseMessage);
        }
    }
}