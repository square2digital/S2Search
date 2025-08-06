using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using System.Data;
using System.Data.SqlClient;

namespace S2Search.Backend.Common.S2Search.Common.Database.Sql.Dapper.S2Search.Common.Database.Sql.Dapper.Providers
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConnectionStringProvider _connectionString;

        public DbConnectionFactory(IConnectionStringProvider connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Create(string connectionName)
        {
            return new SqlConnection(_connectionString.Get(connectionName));
        }
    }
}
