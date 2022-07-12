using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Models.JsonModels
{
    public class ProductsDbObjects
    {
       public List<ShopProduct> products { get; set; } = new List<ShopProduct>();
    }
}
