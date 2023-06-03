using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using S2Search.FeedServices.Function;
using Services;

[assembly: FunctionsStartup(typeof(Startup))]

namespace S2Search.FeedServices.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddFunctionServices();
        }
    }
}
