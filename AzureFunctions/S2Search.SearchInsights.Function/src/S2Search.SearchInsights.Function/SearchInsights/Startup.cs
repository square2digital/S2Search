using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using SearchInsights;
using Services.Extensions;

[assembly: FunctionsStartup(typeof(Startup))]

namespace SearchInsights
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddFunctionServices();
        }
    }
}
