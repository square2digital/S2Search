using S2Search.Backend.Domain.Customer.SearchResources.FeedCredentials;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers
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
