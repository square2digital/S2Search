using Services.Interfaces.Providers;
using System;

namespace Services.Providers
{
    public class GuidProvider : IGuidProvider
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}