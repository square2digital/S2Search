using S2Search.Backend.Domain.Customer.Customer;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<CustomerIds> GetCustomerById(Guid customerId);
    Task<CustomerFull> GetCustomerFull(Guid customerId);
}