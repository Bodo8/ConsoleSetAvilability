using ConsoleSetAvilability.Library.Models;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using ConsoleSetAvilability.Library.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Services.Interfaces
{
    public interface IUpdateProductsByApiService
    {
        Task<List<ResponseReserveProduct>> UpdateProducts(OnlineProductsObject? productsToProcess, Website website, AppSettings appSettings, HttpClient apiClient);
    }
}
