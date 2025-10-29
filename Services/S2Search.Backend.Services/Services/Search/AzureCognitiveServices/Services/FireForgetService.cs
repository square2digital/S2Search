using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;
using System;
using System.Threading.Tasks;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Services
{
    public class FireForgetService<TInterface> : IFireForgetService<TInterface>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<FireForgetService<TInterface>> _logger;

        public FireForgetService(IServiceScopeFactory serviceScopeFactory, ILogger<FireForgetService<TInterface>> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Execute(Func<TInterface, Task> serviceToInvoke)
        {
            // Fire off the task, but don't await the result
            Task.Run(async () =>
            {
                // Exceptions must be caught
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<TInterface>();
                    await serviceToInvoke(repository);
                }
                catch (Exception e)
                {
                    // Only log errors; include UTC timestamp in an ISO8601 format
                    _logger.LogError(e, "FireForgetService exception for {ServiceType} at {UtcNow}: {Message}", typeof(TInterface).FullName, DateTime.UtcNow.ToString("o"), e.Message);
                }
            });
        }
    }
}
