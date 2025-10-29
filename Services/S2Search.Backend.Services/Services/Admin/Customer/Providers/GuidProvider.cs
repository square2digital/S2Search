using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;

namespace S2Search.Backend.Services.Services.Admin.Customer.Providers
{
    public class GuidProvider : IGuidProvider
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}