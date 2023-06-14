using Services.Customer.Interfaces.Providers;

namespace Services.Customer.Providers
{
    public class GuidProvider : IGuidProvider
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}