using System;

namespace S2Search.Backend.Domain.Interfaces.SearchInsights.Providers
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow();
    }
}
