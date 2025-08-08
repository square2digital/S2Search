using Microsoft.Data.SqlClient;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using System.Data;

namespace S2Search.Backend.Services.Services.Admin.Dapper.Providers
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection Create(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
