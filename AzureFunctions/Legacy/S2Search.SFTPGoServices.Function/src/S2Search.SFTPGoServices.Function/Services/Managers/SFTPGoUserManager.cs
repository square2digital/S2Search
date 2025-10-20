using Services.Interfaces.Managers;
using System;
using System.Collections.Generic;
using S2Search.SFTPGo.Client.AutoRest;
using S2Search.SFTPGo.Client.AutoRest.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Domain.AppSettings;
using Services.Interfaces.Repositories;
using Domain.Requests;
using Domain.SFTPGo;
using Domain.Enums;

namespace Services.Managers
{
    public class SFTPGoUserManager : ISFTPGoUserManager
    {
        private readonly ISFTPGoClient _sftpGoClient;
        private readonly AzureSettings _azureSettings;
        private readonly IPasswordHashManager _hashManager;
        private readonly IFeedCredentialsRepository _credentialsRepo;

        private const string accountKeyStatus = "Plain";
        private const int azureFilesystemProvider = 3;
        private const string storageVirtualFolder = "data";
        private readonly IList<string> sftpGoDefaultPermissions = new List<string>() { "list", "download", "upload", "overwrite", "rename", "delete" };

        public SFTPGoUserManager(ISFTPGoClient sftpGoClient,
                                 IOptions<AzureSettings> options,
                                 IPasswordHashManager hashManager,
                                 IFeedCredentialsRepository credentialsRepo)
        {
            _sftpGoClient = sftpGoClient;
            _azureSettings = options.Value;
            _hashManager = hashManager;
            _credentialsRepo = credentialsRepo;
        }

        public async Task<User> CreateUserAsync(CreateUserRequest request)
        {
            return await CreateUserHandler(request);
        }

        private async Task<User> CreateUserHandler(CreateUserRequest request)
        {
            var newUser = GetNewUserConfiguration(request.Username, request.Password);

            var existingUser = await CheckForExistingUser(request.Username);

            if (existingUser)
            {
                throw new Exception($"SFTPGo User '{request.Username}' already exists");
            }

            var createdUser = (User)await _sftpGoClient.Add.UserMethodAsync(newUser);

            if (createdUser != null)
            {
                var passwordHash = GeneratePasswordHash(request.Password);
                await _credentialsRepo.Add(request.SearchIndexId, request.Username, passwordHash);
            }

            return createdUser;
        }

        public async Task UpdatePasswordAsync(UpdatePasswordRequest request)
        {
            await UpdatePasswordHandler(request);
        }

        private async Task UpdatePasswordHandler(UpdatePasswordRequest request)
        {
            var existingUser = await GetUserAsync(request.Username);
            existingUser.Password = request.Password;

            var response = await UpdateUserAsync(existingUser, request.Username);

            if (!string.IsNullOrEmpty(response.Error))
            {
                throw new Exception($"{nameof(UpdatePasswordAsync)} Error: {response}");
            }

            var passwordHash = GeneratePasswordHash(request.Password);
            await _credentialsRepo.Update(request.SearchIndexId, request.Username, passwordHash);
        }

        private static void SetUnusedFilesystemConfigurationsToNull(User existingUser)
        {
            // here we need to check what Filesystem is being used and then null the unused configurations
            // this is so that sftpgo api will not throw a validation error as it is checking for null
            // when getting a user the configuration objects come back as initialised but without any configuration set
            // this is why we need to null them
            var currentProvider = existingUser.Filesystem.Provider.Value;
            var parsedProvider = Enum.Parse<SFTPGoFilesystemProvider>(currentProvider.ToString());

            switch (parsedProvider)
            {
                case SFTPGoFilesystemProvider.AzureBlobStorage:
                    existingUser.Filesystem = new FilesystemConfig()
                    {
                        Azblobconfig = existingUser.Filesystem.Azblobconfig
                    };
                    break;
                case SFTPGoFilesystemProvider.GoogleCloudStorage:
                    existingUser.Filesystem = new FilesystemConfig()
                    {
                        Gcsconfig = existingUser.Filesystem.Gcsconfig
                    };
                    break;
                case SFTPGoFilesystemProvider.S3ObjectStorage:
                    existingUser.Filesystem = new FilesystemConfig()
                    {
                        S3config = existingUser.Filesystem.S3config
                    };
                    break;
                case SFTPGoFilesystemProvider.SFTP:
                    existingUser.Filesystem = new FilesystemConfig()
                    {
                        Sftpconfig = existingUser.Filesystem.Sftpconfig
                    };
                    break;
            }
        }

        public async Task DeleteUserAsync(DeleteUserRequest request)
        {
            await DeleteUserAsyncHandler(request);
        }

        private async Task DeleteUserAsyncHandler(DeleteUserRequest request)
        {
            var userExists = await CheckForExistingUser(request.Username);

            if (!userExists)
            {
                return;
            }

            var response = await _sftpGoClient.Delete.UserMethodAsync(request.Username);

            if (!string.IsNullOrEmpty(response.Error))
            {
                throw new Exception($"Error deleting user. {response.Error}");
            }

            await _credentialsRepo.Delete(request.SearchIndexId, request.Username);
        }

        private string GeneratePasswordHash(string password)
        {
            string passwordHash = _hashManager.GenerateHash(password);

            return passwordHash;
        }

        private User GetNewUserConfiguration(string username, string password)
        {
            return new User
            {
                Username = username,
                Password = password,
                Status = 1, //active
                Permissions = new RootUserPermissions()
                {
                    Permissions = sftpGoDefaultPermissions
                },
                Filesystem = new FilesystemConfig()
                {
                    Provider = azureFilesystemProvider,
                    Azblobconfig = new AzureBlobFsConfig()
                    {
                        Container = _azureSettings.StorageAccountContainer,
                        AccountName = _azureSettings.StorageAccountName,
                        AccountKey = new Secret()
                        {
                            Status = accountKeyStatus,
                            Payload = _azureSettings.StorageAccountKey
                        },
                        KeyPrefix = $"{storageVirtualFolder}/{username}/"
                    }
                }
            };
        }

        private async Task<User> GetUserAsync(string username)
        {
            var user = (User)await _sftpGoClient.Get.Username1Async(username);

            return user;
        }

        private async Task<bool> CheckForExistingUser(string username)
        {
            var response = await _sftpGoClient.Get.Username1WithHttpMessagesAsync(username);
            var userExists = response.Response.IsSuccessStatusCode;

            return userExists;
        }

        public async Task UpdateUserStatusAsync(UpdateUserStatusRequest request)
        {
            await UpdateUserStatusAsyncHandler(request);
        }

        private async Task UpdateUserStatusAsyncHandler(UpdateUserStatusRequest request)
        {
            var existingUser = await GetUserAsync(request.Username);
            existingUser.Status = request.Status ? 1 : 0;

            var response = await UpdateUserAsync(existingUser, request.Username);

            if (!string.IsNullOrEmpty(response.Error))
            {
                throw new Exception($"{nameof(UpdateUserStatusAsyncHandler)} Error: {response}");
            }
        }

        private async Task<ApiResponse> UpdateUserAsync(User existingUser, string username)
        {
            int disconnectUser = 1;
            
            SetUnusedFilesystemConfigurationsToNull(existingUser);

            return await _sftpGoClient.Update.UserMethodAsync(existingUser, username, disconnectUser);
        }
    }
}
