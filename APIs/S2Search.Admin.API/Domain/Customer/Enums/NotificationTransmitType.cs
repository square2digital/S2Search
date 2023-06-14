using System.ComponentModel;

namespace Domain.Customer.Enums
{
    public enum NotificationTransmitType
    {
        [Description("Email")]
        Email,

        [Description("System")]
        System
    }
}
