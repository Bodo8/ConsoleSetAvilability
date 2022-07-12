using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Models.JsonModels
{
    public class RangeNamesObject
    {
        public List<RangeName> RangeNames { get; set; } = new List<RangeName>();
    }

    public class RangeName
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int ShopStatusId { get; set; }
        public int? DelayMinDays { get; set; }
        public int? DelayMaxDays { get; set; }
        public string? ShopNameStatus { get; set; }
    }
}
