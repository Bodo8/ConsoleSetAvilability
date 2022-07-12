using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Models.ConfigurationModels
{
    public class ApiShopSettings
    {
        public bool IsTestMode { get; set; }
        public string BaseUrlShopApi { get; set; }
        public string UrlProductDb { get; set; }
        public string UrlProductReserve { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public string UrlOneProduct { get; set; }
        public string AvailabilityZero { get; set; }
        public string KeyCeneoKupTeraz { get; set; }
        public string KeyGoogleMerchant { get; set; }
    }
}
