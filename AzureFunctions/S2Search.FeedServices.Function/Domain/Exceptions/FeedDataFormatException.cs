using System;

namespace Domain.Exceptions
{
    [Serializable]
    public class FeedDataFormatException : Exception
    {
        public FeedDataFormatException()
        { }

        public FeedDataFormatException(string message)
            : base(message)
        { }

        public FeedDataFormatException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}