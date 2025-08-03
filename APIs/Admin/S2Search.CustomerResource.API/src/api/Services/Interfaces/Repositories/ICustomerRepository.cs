using Domain.Customer;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerById(Guid customerId);
        Task<CustomerFull> GetCustomerFull(Guid customerId);
    }
}