using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Models
{
    public class StatusRangeMath
    {
        public decimal QuantityMax { get; set; }
        public decimal QuantityMin { get; set; }
        public string? NameAvailability { get; set; }
        public int? DaysDelay { get; set; }
    }
}
