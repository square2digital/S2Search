using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Services.Customer.Interfaces.Managers
{
    public interface IFeedUploadManager
    {
        Task<(bool, string)> UploadFileAsync(Guid customerId, Guid searchIndexId, IFormFile file);
    }
}
