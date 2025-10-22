using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Repositories
{
    public interface IFeedCredentialsRepository
    {
        Task Add(Guid searchIndexId, string username, string passwordHash);
        Task Update(Guid searchIndexId, string username, string passwordHash);
        Task Delete(Guid searchIndexId, string username);
    }
}
