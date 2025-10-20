using Domain.AppSettings;
using Microsoft.Extensions.Options;
using Services.Interfaces.Providers;
using System;
using System.Text;

namespace Services.Providers
{
    public class BasicAuthProvider : IBasicAuthProvider
    {
        private readonly SFTPGoSettings _sftpGoSettings;

        public BasicAuthProvider(IOptions<SFTPGoSettings> options)
        {
            _sftpGoSettings = options.Value;
        }

        public string GetAuthToken()
        {
            var authToken = Encoding.ASCII.GetBytes($"{_sftpGoSettings.Username}:{_sftpGoSettings.Password}");
            return Convert.ToBase64String(authToken);
        }
    }
}
