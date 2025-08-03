using System;

namespace Domain.Customer.Dashboard
{
    public class DashboardSummaryItem
    {
        public Guid SearchIndexId { get; set; }
        public string SearchIndexFriendlyName { get; set; }
        public DateTime SearchIndexCreatedDate { get; set; }
        public string NotificationEvent { get; set; }
        public string NotificationCategory { get; set; }
        public DateTime NotificationCreatedDate { get; set; }
        public int SynonymsCount { get; set; }
        public int NotificationsCount { get; set; }
    }
}
