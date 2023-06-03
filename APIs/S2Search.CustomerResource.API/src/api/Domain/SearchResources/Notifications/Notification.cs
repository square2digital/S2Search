using System;
using System.Collections.Generic;

namespace Domain.SearchResources.Notifications
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public Guid SearchIndexId { get; set; }
        public string Event { get; set; }
        public string Category { get; set; }

        private string _recipients;
        public string Recipients
        {
            get { return _recipients; }
            set
            {
                _recipients = value;
                char splitter = ';';
                RecipientsList = _recipients?.Split(splitter);
            }
        }

        public IEnumerable<string> RecipientsList { get; private set; }
        public string TransmitType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
