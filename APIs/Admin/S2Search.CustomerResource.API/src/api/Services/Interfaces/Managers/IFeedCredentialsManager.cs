using Domain.SearchResources.FeedCredentials;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Managers
{
    public interface IFeedCredentialsManager
    {
        Task CreateUser(CreateUserRequest request);
        Task UpdateUserStatus(UpdateUserStatusRequest request);
        Task DeleteUser(DeleteUserRequest request);
        Task UpdatePassword(UpdatePasswordRequest request);
        Task<bool> CheckUserExists(Guid searchIndexId, string username);
        Task<FeedCredentials> GetCredentials(Guid searchIndexId);
    }
}
