using Microsoft.Extensions.Logging;

namespace Domain.Exceptions
{
    public static class EventIds
    {
        // Elastic Errors
        public static readonly EventId ElasticSearchError = new EventId(101, "ElasticSearchError");
        public static readonly EventId ElasticIndexDoesNotExist = new EventId(102, "ElasticIndexDoesNotExist");
        public static readonly EventId SearchDataRequestIsNull = new EventId(103, "SearchDataRequestIsNull");
        public static readonly EventId ElasticIndexIsNull = new EventId(104, "ElasticIndexIsNull");
        public static readonly EventId ElasticSearchTermIsNull = new EventId(105, "ElasticSearchTermIsNull");
        public static readonly EventId ElasticSearchSizeNumberCannotBeZero = new EventId(106, "ElasticSearchSizeNumberCannotBeZero");
        public static readonly EventId ElasticIngestDataError = new EventId(107, "ElasticIngestDataError");
    }
}
