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
    public interface IWebService
    {
        HttpClient GetHttpClient();
        HttpClient LoginToApi(ApiProductSettings apiConnectionSettings);
        Task<T?> GetApiContent<T>(string urlWebsites, HttpClient apiclient);
        Task<ResponseReserveProduct> SetAvailabilityInShop(
           string availabilityName,
           OnlineProduct product,
           ApiShopSettings apiShopSettings,
           List<RangeName> rangeNames,
           HttpClient shopClient);
        Task<ShopProduct> GetCurrentlyShopProduct(OnlineProduct product, ApiShopSettings apiShopSettings);
        Task<string> UpdateMakePut<T>(string v, T item, HttpClient apiClient);
    }
}
