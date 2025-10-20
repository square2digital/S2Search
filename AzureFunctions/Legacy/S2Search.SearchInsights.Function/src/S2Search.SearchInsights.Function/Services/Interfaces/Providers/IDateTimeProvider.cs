using System;

namespace Services.Interfaces.Providers
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow();
    }
}
