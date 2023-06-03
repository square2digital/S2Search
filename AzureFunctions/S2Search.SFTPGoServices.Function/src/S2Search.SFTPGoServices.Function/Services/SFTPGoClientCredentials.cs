using Microsoft.Rest;
using Services.Interfaces.Providers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class SFTPGoClientCredentials : ServiceClientCredentials
    {
        private const string _authorizationHeader = "Authorization";
        private const string _authenticationScheme = "Bearer";
        private readonly IAccessTokenProvider _tokenProvider;

        public SFTPGoClientCredentials(IAccessTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }
        
        public override async Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenProvider.GetOrRefreshAccessTokenAsync();
            request.Headers.Add(_authorizationHeader, $"{_authenticationScheme} {token}");

            await base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}
