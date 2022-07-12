using ConsoleSetAvilability.Library.Models;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using ConsoleSetAvilability.Library.Models.JsonModels;
using ConsoleSetAvilability.Library.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Services
{
    public class SourceService : ISourceService
    {
        private readonly IWebService _webService;

        public SourceService(IWebService webService)
        {
            _webService = webService;
        }

        public async Task<OnlineProductsObject?> GetApiProductContent(HttpClient apiClient, string url)
        {
            OnlineProductsObject? oneResult = await _webService.GetApiContent<OnlineProductsObject>(url, apiClient);

            return oneResult;
        }

        public async Task<T?> GetApiContent<T>(HttpClient apiClient, string url)
        {
            //await SpecialUpdate<T>(apiClient);  //update dla OnlineProduct w razie potrzeby

            return await _webService.GetApiContent<T>(url, apiClient);
        }

        // Poprawa synboli w dwu firmach może się jeszcze przydać
        private async Task SpecialUpdate<T>(HttpClient apiClient)
        {
           //      do testu https://localhost:5003/
            string url = $"https://apipro.grupakrawczyk.pl/api/OnlineProduct/AllIsLatest?WebsiteId=8&IsLatest=false";  //&IsLatest=false;
            string urlPut = "https://apipro.grupakrawczyk.pl/api/OnlineProduct/";
            
            OnlineProductsObject? oneResult = await _webService.GetApiContent<OnlineProductsObject>(url, apiClient);
            
            foreach (OnlineProduct item in oneResult.Products)
            {
                string symbol = item.SymbolAdditional;
                string additional = item.Symbol;
                if(symbol != null)
                item.Symbol = symbol;

                item.SymbolAdditional = additional;
                OnlineProductDto dto = new() { Id = item.Id, Symbol = item.Symbol, WebsiteId = item.WebsiteId, SymbolAdditional = additional  }; 
                var result = await _webService.UpdateMakePut<OnlineProductDto>(urlPut + item.Id.ToString(), dto, apiClient);
            }
        }

        public HttpClient GetHttpClientApiAuthorizeSource(ApiProductSettings apiProductSettings)
        {
            return _webService.LoginToApi(apiProductSettings);
        }

    }
}
