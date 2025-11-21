using Microsoft.AspNetCore.Mvc;
using Npgsql;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Cache;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminStatusController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDistributedCacheService _redisService;
        private readonly string _connectionString;

        public AdminStatusController(IConfiguration configuration, IDistributedCacheService redisService)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString(ConnectionStringKeys.SqlDatabase);
            _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
        }

        [HttpGet(Name = "GetAPIStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok("Ok!");
        }

        [HttpGet("db", Name = "GetDatabaseStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetDatabaseStatus()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                return Ok(new { status = "Database connection successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Database connection failed", error = ex.Message });
            }
        }

        [HttpGet("redis", Name = "GetRedisStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetRedisStatus()
        {
            try
            {
                var status = _redisService.IsConnected() ? "Redis connection successful" : "Redis connection failed";
                return Ok(new { status });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Redis connection failed", error = ex.Message });
            }
        }
    }
}
