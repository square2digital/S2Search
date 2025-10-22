using Domain.Constants;
using Domain.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;
using System.Threading.Tasks;

namespace S2Search.SFTPGoServices.Function
{
    public class CreateUser
    {
        private readonly ISFTPGoUserManager _userManager;

        public CreateUser(ISFTPGoUserManager userManager)
        {
            _userManager = userManager;
        }

        [FunctionName(FunctionNames.CreateUser)]
        public async Task Run([QueueTrigger(StorageQueues.CreateUser, Connection = ConnectionStrings.AzureStorageAccount)] CreateUserRequest request,
                               ILogger log)
        {
            log.LogInformation($"Creating User for SearchIndexId: {request.SearchIndexId}");

            var newUser = await _userManager.CreateUserAsync(request);

            log.LogInformation($"User created: {newUser.Username}");
        }
    }
}
