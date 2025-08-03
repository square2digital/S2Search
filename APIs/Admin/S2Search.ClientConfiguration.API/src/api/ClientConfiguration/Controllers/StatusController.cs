using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace S2Search.ClientConfiguration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet(Name ="GetAPIStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok("Ok!");
        }
    }
}
