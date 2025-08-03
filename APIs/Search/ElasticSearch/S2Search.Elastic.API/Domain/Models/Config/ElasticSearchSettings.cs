using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Config
{
    public class ElasticSearchSettings
    {
        public string Endpoint { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool InvokeWithClientCertificate { get; set; }
    }
}
