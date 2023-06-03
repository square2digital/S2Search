using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Services.Interfaces.Managers
{
    public interface IFeedUploadValidationManager
    {
        Task<(bool, string)> IsValid(IFormFile file);
    }
}
