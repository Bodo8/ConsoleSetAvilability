using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Models.JsonModels
{
    public class OnlineProductsPaginatedObject
    {
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public List<OnlineProduct> Items { get; set; } = new List<OnlineProduct>();
    }

    public class OnlineProductsObject
    {
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public List<OnlineProduct> Products { get; set; } = new List<OnlineProduct>();
    }
}
