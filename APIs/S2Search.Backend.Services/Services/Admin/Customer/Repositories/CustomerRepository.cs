using Microsoft.Extensions.Configuration;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.Customer;
using S2Search.Backend.Domain.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Services.Services.Admin.Customer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDbContextProvider _dbContext;
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration, IDbContextProvider dbContextProvider)
        {
            _dbContext = dbContextProvider;
            _connectionString = configuration.GetConnectionString(ConnectionStringKeys.SqlDatabase);
        }

        public async Task<CustomerIds> GetCustomerById(Guid customerId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerId", customerId }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<CustomerIds>(_connectionString, StoredProcedures.GetCustomerById, parameters);

            return result;
        }

        public async Task<CustomerFull> GetCustomerFull(Guid customerId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerId", customerId }
            };

            var result = await _dbContext.QueryMultipleAsync<CustomerFull>(_connectionString, StoredProcedures.GetCustomerFull, parameters);

            return result;
        }
    }
}
