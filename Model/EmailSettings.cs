using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arfler.Models
{
    public class EmailSettings
    {
        public string ApiKey { get; set; }
        public string BaseUri { get; set; }
        public string RequestUri { get; set; }
        public string From { get; set; }
        public string Hostname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
