using Domain.Constants;
using Domain.Customer;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using Services.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbContextProvider _dbContext;

        public CustomerRepository(IDbContextProvider dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Customer> GetCustomerById(Guid customerId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerId", customerId }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<Customer>(ConnectionStrings.CustomerResourceStore, StoredProcedures.GetCustomerById, parameters);

            return result;
        }

        public async Task<CustomerFull> GetCustomerFull(Guid customerId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerId", customerId }
            };

            var result = await _dbContext.QueryMultipleAsync<CustomerFull>(ConnectionStrings.CustomerResourceStore, StoredProcedures.GetCustomerFull, parameters);

            return result;
        }
    }
}
