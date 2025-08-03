using Microsoft.Extensions.Configuration;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using System;

namespace S2Search.Common.Database.Sql.Dapper.Providers
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _config;

        public ConnectionStringProvider(IConfiguration config)
        {
            _config = config;
        }

        public string Get(string connectionName)
        {
            string connectionString = _config[connectionName];

            if (string.IsNullOrEmpty(connectionString)) throw new Exception($"Connectiong string for key '{connectionName}' not found");

            return connectionString;
        }
    }
}
