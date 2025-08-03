using Domain.Customer;
using Domain.Customer.Customer;

namespace Services.Customer.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<CustomerIds> GetCustomerById(Guid customerId);
        Task<CustomerFull> GetCustomerFull(Guid customerId);
    }
}