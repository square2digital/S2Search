using Microsoft.Rest;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Extensions
{
    public class ClientConfigurationApiClientCredentials : ServiceClientCredentials
    {
        private const string _authorizationHeader = "Authorization";
        private const string _authenticationScheme = "Basic";

        public ClientConfigurationApiClientCredentials()
        {

        }

        public override async Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add(_authorizationHeader, $"{_authenticationScheme} test");

            await base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}
