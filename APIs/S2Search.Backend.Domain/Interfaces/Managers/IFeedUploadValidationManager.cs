using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

public interface IFeedUploadValidationManager
{
    Task<(bool, string)> IsValid(IFormFile file);
}
