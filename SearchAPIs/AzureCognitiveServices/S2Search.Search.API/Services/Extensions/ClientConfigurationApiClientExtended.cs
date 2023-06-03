using Domain.Models.Interfaces;
using S2Search.ClientConfigurationApi.Client.AutoRest;
using System;
using System.Net.Http;

namespace Services.Extensions
{
    public partial class ClientConfigurationApiClientExtended : ClientConfigurationApiClient
    {
        public ClientConfigurationApiClientExtended(HttpClient httpClient,
                                     IAppSettings appsettings) : base(new AnonymousClientCredentials(), httpClient, false)
        {
            BaseUri = new Uri(appsettings.ClientConfigurationSettings.ClientConfigurationEndpoint);           
        }
    }
}
