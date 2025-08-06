using Microsoft.AspNetCore.Mvc;
using Nest;
using S2Search.Backend.Domain.Search.Elastic.Exceptions;
using S2Search.Backend.Domain.Search.Elastic.Interfaces;
using S2Search.Backend.Domain.Search.Elastic.Models.Request;

namespace S2Search.Backend.Controllers.Search.Elastic
{
    [Route("v1/[controller]")]
    public class BaseController : Controller
    {
        private readonly IElasticIndexService _elasticIndexService;
        private readonly IAppSettings _appSettings;
        private readonly ILogger _logger;

        private string errorMessage = string.Empty;

        public BaseController(IAppSettings appSettings,
            ILogger logger,
            IElasticIndexService elasticIndexService)
        {
            _elasticIndexService = elasticIndexService ?? throw new ArgumentNullException(nameof(elasticIndexService));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));            
        }

        /// <summary>
        /// This will be temporary
        /// </summary>
        /// <param name="request"></param>
        private void NullVehicleIndexFallback(SearchDataRequest request)
        {
            if (string.IsNullOrEmpty(request.Index))
            {
                request.Index = "s2-demo-vehicles";
            }
        }

        protected void ValidateIndex(string index, string actionName)
        {
            if (string.IsNullOrEmpty(index))
            {
                errorMessage = $"Error on {actionName} | Message: The index is compulsory";
                var exception = new ElasticSearchException(errorMessage);
                _logger.LogError(EventIds.ElasticIndexIsNull, exception, exception.Message);
                throw exception;
            }

            if(!_elasticIndexService.DoesIndexExist(index))
            {
                errorMessage = $"Error on {actionName} | Message: Index '{index}' does not exist";
                var exception = new ElasticSearchException(errorMessage);
                _logger.LogError(EventIds.ElasticIndexDoesNotExist, exception, exception.Message);
                throw exception;
            }
        }

        protected void ValidateSearchDataRequest(SearchDataRequest request, string actionName)
        {
            if (request == null)
            {
                errorMessage = $"Error on {actionName} | Message: The SearchDataRequest cannot be null";
                var exception = new ElasticSearchException(errorMessage);
                _logger.LogError(EventIds.SearchDataRequestIsNull, exception, exception.Message);
                throw exception;
            }

            NullVehicleIndexFallback(request);

            if (string.IsNullOrEmpty(request.Index))
            {
                errorMessage = $"Error on {actionName} | Message: request.Index is null - The index is compulsory";
                var exception = new ElasticSearchException(errorMessage);
                _logger.LogError(EventIds.ElasticIndexIsNull, exception, exception.Message);
                throw exception;
            }

            if (!_elasticIndexService.DoesIndexExist(request.Index))
            {
                errorMessage = $"Error on {actionName} | Message: Index '{request.Index}' does not exist";
                var exception = new ElasticSearchException(errorMessage);
                _logger.LogError(EventIds.ElasticIndexDoesNotExist, exception, exception.Message);
                throw exception;
            }

            if (request.PageSize == 0)
            {
                errorMessage = $"Error on {actionName} | Message: The Page Size cannot be zero";
                var exception = new ElasticSearchException(errorMessage);
                _logger.LogError(EventIds.ElasticSearchSizeNumberCannotBeZero, exception, exception.Message);
                throw exception;
            }
        }

        protected void LogApiDetails()
        {
            var requestUrl = $"{Request.Scheme}://{Request.Host.Value}{Request.Path}";
            _logger.LogInformation($"requestUrl -> {requestUrl}{Request.QueryString.Value}");
        }
    }
}
