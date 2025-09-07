using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.Customer;
using S2Search.Backend.Domain.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Services.Services.Admin.Customer.Repositories
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

            var result = await _dbContext.QuerySingleOrDefaultAsync<CustomerIds>(ConnectionStrings.S2_Search, StoredProcedures.GetCustomerById, parameters);

            return result;
        }

        public async Task<CustomerFull> GetCustomerFull(Guid customerId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "CustomerId", customerId }
            };

            var result = await _dbContext.QueryMultipleAsync<CustomerFull>(ConnectionStrings.S2_Search, StoredProcedures.GetCustomerFull, parameters);

            return result;
        }
    }
}
