using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Nest;

namespace Services.Providers
{
    public class ElasticSearchClientProvider : IElasticSearchClientProvider
    {
        private readonly IAppSettings _appSettings;
        private readonly ILogger<ElasticSearchClientProvider> _logger;

        public ElasticSearchClientProvider(IAppSettings appSettings, ILogger<ElasticSearchClientProvider> logger)
        {
            _appSettings = appSettings;
            _logger = logger;
        }

        public ElasticClient GetElasticClient()
        {
            try
            {
                ConnectionSettings settings = GetConnectionSettings();

                if (_appSettings.Development)
                {
                    settings.EnableApiVersioningHeader();
                    settings.EnableDebugMode();
                }

                return new ElasticClient(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                throw;
            }
        }

        private ConnectionSettings GetConnectionSettings()
        {
            ConnectionSettings settings;

            if (HasCredentials())
            {
                settings = new ConnectionSettings(new Uri(_appSettings.ElasticSearchSettings.Endpoint))
                    .BasicAuthentication(_appSettings.ElasticSearchSettings.Username,
                                         _appSettings.ElasticSearchSettings.Password);
            }
            else
            {
                settings = new ConnectionSettings(new Uri(_appSettings.ElasticSearchSettings.Endpoint));
            }

            return settings;
        }

        private bool HasCredentials()
        {
            try
            {
                if (string.IsNullOrEmpty(_appSettings.ElasticSearchSettings.Username)
                    || string.IsNullOrEmpty(_appSettings.ElasticSearchSettings.Password))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                throw;
            }
        }
    }
}