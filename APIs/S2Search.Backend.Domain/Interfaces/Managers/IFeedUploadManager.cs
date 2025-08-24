using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

public interface IFeedUploadManager
{
    Task<(bool, string)> UploadFileAsync(Guid customerId, Guid searchIndexId, IFormFile file);
}
