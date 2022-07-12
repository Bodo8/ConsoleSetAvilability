using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Models.JsonModels
{
    public class ResponseReserveProduct
    {
        public bool IsUpdated { get; set; }
        public bool IsSynchronize { get; set; }
        public string NameAvailability { get; set; }
        public string Symbol { get; set; }
        public bool WasSetsToZero { get; set; }
        public bool CeneoBuyNow { get; set; }
        public bool GoogleMerchand { get; set; }
        public string Message { get; set; }
    }
}
