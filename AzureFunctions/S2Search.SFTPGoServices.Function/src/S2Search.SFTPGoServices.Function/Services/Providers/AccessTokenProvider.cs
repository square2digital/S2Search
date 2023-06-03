using Domain.AppSettings;
using Domain.Responses;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.Interfaces.Providers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Services.Providers
{
    public class AccessTokenProvider : IAccessTokenProvider
    {
        private string _accessToken;
        private DateTime _tokenExpiry;
        private readonly HttpClient _httpClient;
        private readonly SFTPGoSettings _sftpGoSettings;

        public AccessTokenProvider(HttpClient httpClient, IOptions<SFTPGoSettings> options)
        {
            _httpClient = httpClient;
            _sftpGoSettings = options.Value;
        }

        public async Task<string> GetOrRefreshAccessTokenAsync()
        {
            if (string.IsNullOrEmpty(_accessToken) || DateTime.UtcNow > _tokenExpiry)
            {
                await GetAccessToken();
            }

            return _accessToken;
        }

        private async Task GetAccessToken()
        {
            var response = await _httpClient.GetAsync(_sftpGoSettings.TokenUrl);
            var responseContent = await response.Content.ReadAsStringAsync();

            var accessToken = JsonConvert.DeserializeObject<AccessTokenResponse>(responseContent);

            _accessToken = accessToken.AccessToken;
            _tokenExpiry = accessToken.Expires;
        }
    }
}
