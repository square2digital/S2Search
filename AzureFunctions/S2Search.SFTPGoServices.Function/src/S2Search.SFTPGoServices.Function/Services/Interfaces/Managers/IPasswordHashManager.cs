using System.Security;

namespace Services.Interfaces.Managers
{
    public interface IPasswordHashManager
    {
        string GenerateHash(string password);
        bool VerifyHash(string password, string passwordHash);
    }
}
