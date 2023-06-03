using System.Threading.Tasks;
using Domain.Constants;
using Domain.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;

namespace S2Search.SFTPGoServices.Function
{
    public class DeleteUser
    {
        private readonly ISFTPGoUserManager _userManager;

        public DeleteUser(ISFTPGoUserManager userManager)
        {
            _userManager = userManager;
        }

        [FunctionName(FunctionNames.DeleteUser)]
        public async Task Run([QueueTrigger(StorageQueues.DeleteUser, Connection = ConnectionStrings.AzureStorageAccount)] DeleteUserRequest request, ILogger log)
        {
            log.LogInformation($"Deleting User: {request.Username}");

            await _userManager.DeleteUserAsync(request);

            log.LogInformation($"Deleted User: {request.Username}");
        }
    }
}
