using System.Threading.Tasks;
using Domain.Constants;
using Domain.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Services.Interfaces.Managers;

namespace S2Search.SFTPGoServices.Function
{
    public class UpdateUserPassword
    {
        private readonly ISFTPGoUserManager _userManager;

        public UpdateUserPassword(ISFTPGoUserManager userManager)
        {
            _userManager = userManager;
        }

        [FunctionName(FunctionNames.UpdateUserPassword)]
        public async Task Run([QueueTrigger(StorageQueues.UpdateUserPassword, Connection = ConnectionStrings.AzureStorageAccount)] UpdatePasswordRequest request, ILogger log)
        {
            log.LogInformation($"Updating password for User: {request.Username}");

            await _userManager.UpdatePasswordAsync(request);

            log.LogInformation($"Updated password updated for User: {request.Username}");

        }
    }
}
