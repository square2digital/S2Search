using Services.Dapper.Interfaces.Providers;
using System.Data;
using System.Data.SqlClient;

namespace Services.Dapper.Providers
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
