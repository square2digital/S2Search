using System.ComponentModel;

namespace S2Search.Backend.Domain.Customer.Enums
{
    public enum NotificationTransmitType
    {
        [Description("Email")]
        Email,

        [Description("System")]
        System
    }
}
