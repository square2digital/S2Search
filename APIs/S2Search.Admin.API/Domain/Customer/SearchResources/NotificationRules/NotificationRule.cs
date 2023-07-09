using Domain.Customer.Enums;

namespace Domain.Customer.SearchResources.NotificationRules
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