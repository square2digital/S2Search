using System;

namespace Services.Customer.Interfaces.Providers
{
    public interface IDateTimeProvider
    {
        DateTime CurrentDateTime();
    }
}
