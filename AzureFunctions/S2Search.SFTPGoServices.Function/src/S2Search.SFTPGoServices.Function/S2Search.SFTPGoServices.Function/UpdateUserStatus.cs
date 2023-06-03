using System.Threading.Tasks;
using Domain.Constants;
using Domain.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;

namespace S2Search.SFTPGoServices.Function
{
    public class UpdateUserStatus
    {
        private readonly ISFTPGoUserManager _userManager;
        public UpdateUserStatus(ISFTPGoUserManager userManager)
        {
            _userManager = userManager;
        }

        [FunctionName(FunctionNames.UpdateUserStatus)]
        public async Task Run([QueueTrigger(StorageQueues.UpdateUserStatus, Connection = ConnectionStrings.AzureStorageAccount)] UpdateUserStatusRequest request, ILogger log)
        {
            log.LogInformation($"Updating status of user: {request.Username}");

            await _userManager.UpdateUserStatusAsync(request);

            log.LogInformation($"Updated status of user: {request.Username}");

        }
    }
}
