using System.ComponentModel;

namespace Domain.Customer.Enums
{
    public enum NotificationTriggerType
    {
        [Description("Feed Success")]
        Feed_Success,
        
        [Description("Feed Failure")]
        Feed_Failure,

        [Description("Feed Manual Upload")]
        Feed_Manual_Upload
    }
}
