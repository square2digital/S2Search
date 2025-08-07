using S2Search.Backend.Common.S2Search.Common.Models.SearchResource.Enums;
using S2Search.Backend.Domain.Admin.SearchResource.Enums;
using S2Search.Backend.Domain.SearchResource.Enums;
using System.Collections.Generic;

namespace S2Search.Backend.Common.S2Search.Common.Models.SearchResource
{
    public class NotificationRule
    {
        public int NotificationRuleId { get; set; }
        public NotificationTransmitType TransmitType { get; set; }
        
        private string _recipients;
        public string Recipients
        {
            get { return _recipients; }
            set
            {
                _recipients = value;
                char splitter = ';';
                RecipientsList = _recipients.Split(splitter);
            }
        }

        public IEnumerable<string> RecipientsList { get; private set; }
        public NotificationTriggerType TriggerType { get; set; }
    }
}
