using System;

namespace Domain.SearchResources.NotificationRules
{
    public class NotificationRuleRequest
    {
        public Guid SearchIndexId { get; set; }
        public string TransmitType { get; set; }
        public string Recipients { get; set; }
        public string TriggerType { get; set; }
    }
}
