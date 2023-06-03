using Newtonsoft.Json;
using System;

namespace Domain.Responses
{
    public class AccessTokenResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expires_at")]
        public DateTime Expires { get; set; }
    }
}
