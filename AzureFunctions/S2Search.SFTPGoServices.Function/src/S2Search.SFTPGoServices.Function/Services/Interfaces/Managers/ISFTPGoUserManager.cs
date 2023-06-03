using Domain.Requests;
using S2Search.SFTPGo.Client.AutoRest.Models;
using System.Threading.Tasks;

namespace Services.Interfaces.Managers
{
    public interface ISFTPGoUserManager
    {
        Task<User> CreateUserAsync(CreateUserRequest request);
        Task UpdatePasswordAsync(UpdatePasswordRequest request);
        Task UpdateUserStatusAsync(UpdateUserStatusRequest request);
        Task DeleteUserAsync(DeleteUserRequest request);
    }
}
