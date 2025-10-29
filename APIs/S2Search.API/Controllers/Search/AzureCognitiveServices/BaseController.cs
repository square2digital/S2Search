using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using S2Search.Backend.Domain.Models.Request;

namespace S2Search.Backend.Controllers.Search.AzureCognitiveServices
{
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Validate the incoming request object using model state.
        /// Returns null when the request is valid. Otherwise returns an IActionResult (BadRequest) that can be returned by the caller.
        /// </summary>
        protected IActionResult? ValidateRequest<T>(T? request) where T : class
        {
            if (request == null)
            {
                return BadRequest();
            }
    
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return null;
        }
    }
}