using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Customer.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T item);
        Task AddAsync(T item);
        void Update(T item);
        Task UpdateAsync(T item);
        void Delete(Guid Id);
        Task DeleteAsync(Guid Id);
        T Get(Guid Id);
        IEnumerable<T> GetItems(string procedureName, object parameters = null);
        bool Exists(T item);
    }
}
