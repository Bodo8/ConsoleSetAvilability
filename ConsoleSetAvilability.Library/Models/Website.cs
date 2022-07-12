using ConsoleSetAvilability.Library.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Models
{
    public class Website
    {
        public int Id { get; set; }
        public string WebsiteName { get; set; }
        public bool CheckAvailability { get; set; }
        public virtual List<StatusRange> StatusRanges { get; set; } = new List<StatusRange>();
    }


}
