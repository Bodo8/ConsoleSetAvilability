using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Models.ConfigurationModels
{
    public class ApiProductSettings
    {
        public bool IsTestMode { get; set; }
        public string LoginUrl { get; set; }
        public string BaseUrl { get; set; }
        public string UrlWebsites { get; set; }
        public string UrlProduct { get; set; }
        public string UrlNamesRange { get; set; }
        public Dictionary<string, string> LoginTokens { get; set; }
        public Dictionary<string, string> RegexLoginTokens { get; set; }
        public string UrlPrinciple { get; set; }
    }
}
