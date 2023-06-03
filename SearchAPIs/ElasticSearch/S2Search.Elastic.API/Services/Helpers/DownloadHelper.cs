using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public static class DownloadHelper
    {
        public static async Task<string> DownloadJson(string url)
        {
            string jsondata = string.Empty;

            using (var webClient = new HttpClient())
            {
                jsondata = await webClient.GetStringAsync(url);
            }

            return jsondata;
        }
    }
}
