using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Dapper.Providers;
using System.Data.SqlClient;

namespace Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDbContextProvider _dbContextProvider;

        public StatusController(IConfiguration configuration, IDbContextProvider dbContextProvider)
        {
            _configuration = configuration;
            _dbContextProvider = dbContextProvider;
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
            var connectionString = _configuration.GetConnectionString("CustomerResourceStore");
            try
            {
                var connStr = "Server=tcp:s2-sql-dev.database.windows.net,1433;Initial Catalog=CustomerResourceStore;Persist Security Info=False;User ID=jgilmartin;Password=y5j5G5jikDdy5jtSUd#ZhP3x48m@7nVwxRg5YA$QUKaYrf%456Gd@w£d*h45QUKaY;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using var connection = new SqlConnection(connStr);
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
