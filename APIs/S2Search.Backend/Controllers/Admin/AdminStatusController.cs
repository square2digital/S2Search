using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminStatusController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AdminStatusController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("S2_Search");
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
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                return Ok(new { status = "Database connection successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Database connection failed", error = ex.Message });
            }
        }
    }
}
