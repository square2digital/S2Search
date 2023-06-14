using Domain.Constants;
using Domain.Customer.Constants;
using Domain.Customer.Customer;
using Services.Customer.Interfaces.Repositories;
using Services.Dapper.Interfaces.Providers;

namespace Services.Customer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbContextProvider _dbContext;

        public CustomerRepository(IDbContextProvider dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<CustomerIds> GetCustomerById(Guid customerId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerId", customerId }
            };

            var result = await _dbContext.QuerySingleOrDefaultAsync<CustomerIds>(ConnectionStrings.CustomerResourceStore, StoredProcedures.GetCustomerById, parameters);

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
