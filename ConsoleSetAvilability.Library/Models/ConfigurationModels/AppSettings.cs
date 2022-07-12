using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Models.ConfigurationModels
{
    public class AppSettings
    {
        public ApiProductSettings? ApiProductData { get; set; }
        public ApiShopSettings? ApiShopData { get; set; }
        public EmailSettings? EmailParameters { get; set; }
        public List<string> EmailRecipients { get; set; }
        public int PercentSetToZero { get; set; }
        public bool RunCheckCheckboxes { get; set; }
        public bool RunCheckAvailability { get; set; }
    }
}
