using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Models.JsonModels
{
    public class PrinciplesObject
    {
        public List<PrincipleCheckbox> PrincipleCheckboxes { get; set; } = new List<PrincipleCheckbox>();
    }

    public class PrincipleCheckbox
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal? PriceGross { get; set; }

        public decimal? Stock { get; set; }

        public bool CeneoBuyNow { get; set; }
        public bool GoogleMarchent { get; set; }
        public List<SymbolProduct> Symbols { get; set; }
    }

    public class SymbolProduct
    {
        public int SymbolProductId { get; set; }
        public string Symbol { get; set; }
    }
}
