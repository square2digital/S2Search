using Domain.Constants;
using Domain.SearchResources.FeedCredentials;
using Services.Interfaces.Managers;
using Services.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class FeedCredentialsManager : IFeedCredentialsManager
    {
        private readonly IQueueManager _queueManager;
        private readonly IFeedCredentialsRepository _credentialsRepo;

        public FeedCredentialsManager(IQueueManager queueManager, IFeedCredentialsRepository credentialsRepo)
        {
            _queueManager = queueManager;
            _credentialsRepo = credentialsRepo;
        }

        public async Task<bool> CheckUserExists(Guid searchIndexId, string username)
        {
            var exists = await _credentialsRepo.CheckUserExists(searchIndexId, username);
            return exists;
        }

        public async Task CreateUser(CreateUserRequest request)
        {
            var message = _queueManager.CreateMessage(SFTPGoQueues.CreateUser, request);
            await _queueManager.EnqueueMessageAsync(message);
        }

        public async Task DeleteUser(DeleteUserRequest request)
        {
            var message = _queueManager.CreateMessage(SFTPGoQueues.DeleteUser, request);
            await _queueManager.EnqueueMessageAsync(message);
        }

        public async Task<FeedCredentials> GetCredentials(Guid searchIndexId)
        {
            var credentials = await _credentialsRepo.GetCredentials(searchIndexId);
            return credentials;
        }

        public async Task UpdatePassword(UpdatePasswordRequest request)
        {
            var message = _queueManager.CreateMessage(SFTPGoQueues.UpdateUserPassword, request);
            await _queueManager.EnqueueMessageAsync(message);
        }

        public async Task UpdateUserStatus(UpdateUserStatusRequest request)
        {
            var message = _queueManager.CreateMessage(SFTPGoQueues.UpdateUserStatus, request);
            await _queueManager.EnqueueMessageAsync(message);
        }
    }
}
