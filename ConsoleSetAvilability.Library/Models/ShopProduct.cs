using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Models
{
    public class ShopProduct
    {
        public int InternalId { get; set; }
        public string Symbol { get; set; }
        public bool Visible { get; set; }
        public bool Unavailable { get; set; }
        public string? availabilityName { get; set; }
        public int AvailabilityId { get; set; }
        public string Name { get; set; }
        public decimal Stock { get; set; }
        public decimal? PriceNet { get; set; }
        public decimal? PriceGross { get; set; }
        public decimal? PricePromoNet { get; set; }
        public decimal? PricePromoGross { get; set; }
        public decimal? PricePromoNotLogNet { get; set; }
        public decimal? PricePromoNotLogGross { get; set; }
        public decimal? PriceGroupRebateGross { get; set; }
        public bool IsCeneoBuyNow { get; set; }
        public bool IsGoogleMerchant { get; set; }
    }
}
