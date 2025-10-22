using Domain.AppSettings;
using Microsoft.Extensions.Options;
using S2Search.SFTPGo.Client.AutoRest;
using Services.Interfaces.Providers;
using System;
using System.Net.Http;

namespace Services.Extensions
{
    public partial class SFTPGoClientExtended : SFTPGoClient
    {
        public SFTPGoClientExtended(HttpClient httpClient,
                                     IOptions<SFTPGoSettings> options,
                                     IAccessTokenProvider tokenProvider) : base(new SFTPGoClientCredentials(tokenProvider), httpClient, false)
        {
            BaseUri = new Uri(options.Value.BaseUrl);
        }
    }
}