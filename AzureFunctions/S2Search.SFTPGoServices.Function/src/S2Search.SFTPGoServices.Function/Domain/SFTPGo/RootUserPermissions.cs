using Newtonsoft.Json;
using System.Collections.Generic;

namespace Domain.SFTPGo
{
    public class RootUserPermissions
    {
        [JsonProperty("/")]
        public IList<string> Permissions { get; set; }
    }
}
