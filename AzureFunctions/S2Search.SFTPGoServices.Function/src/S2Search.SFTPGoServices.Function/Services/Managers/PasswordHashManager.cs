using Microsoft.AspNetCore.Identity;
using Services.Interfaces.Managers;
using System;

namespace Services.Managers
{
    public class PasswordHashManager : IPasswordHashManager
    {
        public string GenerateHash(string password)
        {
            var passwordHash = new PasswordHasher<object>().HashPassword(null, password);
            return passwordHash;
        }

        public bool VerifyHash(string password, string passwordHash)
        {
            var result = new PasswordHasher<object>().VerifyHashedPassword(null, passwordHash, password);

            return result switch
            {
                PasswordVerificationResult.Failed => false,
                PasswordVerificationResult.Success => true,
                PasswordVerificationResult.SuccessRehashNeeded => true,
                _ => throw new Exception("Failed to verify password hash"),
            };
        }
    }
}
