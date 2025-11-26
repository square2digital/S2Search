using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using S2Search.CacheManager.Interfaces.Processors;

namespace S2Search.CacheManager.Services
{
    public class PurgeCache : BackgroundService
    {
        private readonly IPurgeCacheProcessor _processor;
        private readonly ILogger<PurgeCache> _logger;

        public PurgeCache(IPurgeCacheProcessor processor, ILogger<PurgeCache> logger)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PurgeCacheBackgroundService starting.");

            try
            {
                // Call the processor and let it run until the host signals cancellation.
                // If the processor is not long‑running, consider wrapping this call in a loop with a backoff.
                await _processor.RunAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // expected during shutdown
                _logger.LogInformation("PurgeCacheBackgroundService cancellation requested.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in PurgeCacheBackgroundService.");
                throw;
            }

            _logger.LogInformation("PurgeCacheBackgroundService stopping.");
        }
    }
}