using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Models.ConfigurationModels
{
    internal class OnlineProductDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string? SymbolIS { get; set; }
        public string? Name { get; set; }
        public decimal? Quantity { get; set; }
        public int WebsiteId { get; set; }
        public DateTime? DateDelivery { get; set; }
        public string? SymbolAdditional { get; set; }
    }
}
