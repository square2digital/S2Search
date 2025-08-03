using System;

namespace Services.Interfaces.Providers
{
    public interface IGuidProvider
    {
        Guid NewGuid();
    }
}
