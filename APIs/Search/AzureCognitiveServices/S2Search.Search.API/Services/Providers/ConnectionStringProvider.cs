using Microsoft.Extensions.Configuration;
using Services.Interfaces;
using System;

namespace Services.Providers
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _config;

        public ConnectionStringProvider(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public string Get(string connectionName)
        {
            string connectionString = _config[connectionName];

            if (string.IsNullOrEmpty(connectionString)) throw new Exception($"Connectiong string for key '{connectionName}' not found");

            return connectionString;
        }
    }
}
