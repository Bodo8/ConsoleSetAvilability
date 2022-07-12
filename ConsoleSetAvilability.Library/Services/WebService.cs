
using ConsoleSetAvilability.Library.Logic.Interfaces;
using ConsoleSetAvilability.Library.Models;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using ConsoleSetAvilability.Library.Models.JsonModels;
using ConsoleSetAvilability.Library.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleSetAvilability.Library.Services
{
    public class WebService : IWebService
    {
        private readonly IWebShopService _webShopService;
        private readonly IWebProcessor _webProcessor;
        private readonly ILogger<IWebService> _logger;

        public WebService(IWebProcessor webProcessor, ILogger<IWebService> logger, IWebShopService webShopService)
        {
            _webProcessor = webProcessor;
            _logger = logger;
            _webShopService = webShopService;
        }

        public async Task<T?> GetApiContent<T>(string urlGet, HttpClient apiclient)
        {
            string jsonApi = await _webProcessor.MakeGetRequest(urlGet, apiclient);

            return JsonConvert.DeserializeObject<T>(jsonApi);
        }

        public HttpClient GetHttpClient()
        {
            return _webProcessor.GetHttpClient();
        }

        public async Task<ResponseReserveProduct> SetAvailabilityInShop(
           string availabilityName,
           OnlineProduct product,
           ApiShopSettings apiShopSettings,
           List<RangeName> rangeNames,
           HttpClient shopClient)
        {
            ResponseReserveProduct responseReserve = new ResponseReserveProduct() { Symbol = product.Symbol, NameAvailability = availabilityName };
            ShopProduct shopProduct = await GetCurrentlyShopProduct(product, apiShopSettings);
            
            bool isStockShopZero = CheckName(availabilityName, shopProduct.Stock, responseReserve);
            bool doNeedSetNewAvailability = CheckDoNeedSetNewAvailability(shopProduct, availabilityName, rangeNames);

            if (doNeedSetNewAvailability && isStockShopZero)
            {
                string url = apiShopSettings.BaseUrlShopApi + apiShopSettings.UrlProductReserve;
                HttpContent? updatedParameters = _webShopService.CreateParameters(product, availabilityName, rangeNames);
                responseReserve = await _webShopService.MakePostWithParameters(url, updatedParameters, shopClient, responseReserve);

                return responseReserve;
            }
            else
            {
                string info = $"Już było ustawione: {availabilityName} jeśli 'False'={doNeedSetNewAvailability}, lub zapas powyżej zera {isStockShopZero}";
                responseReserve.Message = info;
                return responseReserve;
            }
        }

        public bool CheckDoNeedSetNewAvailability(ShopProduct shopProduct, string availabilityName, List<RangeName> rangeNames)
        {
            if (shopProduct.Unavailable || !shopProduct.Visible)
            {
                _logger.LogWarning($"Product {shopProduct.Symbol} widoczny: {shopProduct.Visible} lub wycofany: {shopProduct.Unavailable}");
                return false;
            }

            int? idAvailabilityonline = GetIdAvailability(availabilityName, rangeNames);

            if (idAvailabilityonline == null)
            {
                _logger.LogError($"Brak id dostępności{availabilityName}");
                return false;
            }

            if (shopProduct.AvailabilityId == idAvailabilityonline)
                return false;
            else
                return true;
        }

        public HttpClient LoginToApi(ApiProductSettings apiConnectionSettings)
        {
            Dictionary<string, string> tokensFromLogin = apiConnectionSettings.LoginTokens;
            Dictionary<string, string> regexLogin = apiConnectionSettings.RegexLoginTokens;
            HttpClient clientApi = GetHttpClient();
            string loginUrl = apiConnectionSettings.BaseUrl + apiConnectionSettings.LoginUrl;
            string response = "";
            bool connectionFail = false;

            do
            {
                try
                {
                    response = _webProcessor.LoginToApi(loginUrl, ref clientApi);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Nie można zlogować do Api" + ex.Message);
                    connectionFail = true;
                }
                if (!response.Contains("token"))
                    connectionFail = true;

                if (connectionFail)
                {
                    _logger.LogInformation($"Czekam na zalogowanie do Api {DateTime.Now.ToString()}");
                    Thread.Sleep(1000 * 60 * 15); //15 minut
                }

            } while (connectionFail);

            string token = GetToken(response, regexLogin["TokenRegex"]);
            tokensFromLogin["Authorization"] = tokensFromLogin["Authorization"] + token;

            foreach (var header in tokensFromLogin)
            {
                clientApi.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            _logger.LogInformation($"zalogowano do Api");

            return clientApi;
        }

        public async Task<ShopProduct> GetCurrentlyShopProduct(OnlineProduct product, ApiShopSettings apiShopSettings)
        {
            HttpClient client = GetHttpClient();
            string symbol = product.Symbol.Trim();
            string url = apiShopSettings.BaseUrlShopApi + apiShopSettings.UrlOneProduct + symbol;
            ShopProduct? shopProduct = await GetApiContent<ShopProduct>(url, client);

            if (shopProduct != null && shopProduct.Symbol != null)
            {
                return shopProduct;
            }
            else
            {
                _logger.LogError($"Nie Znaleziono productu z symbolem: {product.Symbol}");
                //throw new NotFoundException($"Nie znaleziono productu z symbolem: {product.Symbol}");
                return new ShopProduct() { Symbol = product.Symbol };  //zwróci logWarning że product niewidoczny
            }
        }

        private string GetToken(string response, string regexPattern)
        {
            var regex = new Regex(regexPattern);
            var matched = regex.Match(response);

            return matched.Groups["wartosc"].Value;
        }

        public int? GetIdAvailability(string word, List<RangeName> rangeNames)
        {
            var data = rangeNames.Where(r => r.Status == word).FirstOrDefault();

            if (data == null)
                return null;

            return data.ShopStatusId;
        }

        private bool CheckName(string nameAvailability, decimal stock, ResponseReserveProduct responseReserve)
        {
            bool isStockShopZero = true;
            bool doCheckStockToZero = nameAvailability == "Dostępność na zapytanie" || nameAvailability == "Brak";

            if (doCheckStockToZero)
            {
                isStockShopZero = stock == 0;
                responseReserve.WasSetsToZero = true;
            }

            return isStockShopZero;
        }

        public async Task<string> UpdateMakePut<T>(string url, T item, HttpClient apiClient)
        {
            HttpContent httpContent = CreateParameters(item);
            return await _webProcessor.MakePutRequestAsync(url, httpContent, apiClient);
        }

        private HttpContent CreateParameters<T>(T product)
        {
            string jsonData = JsonConvert.SerializeObject(product);

            return new StringContent(jsonData, Encoding.UTF8, "application/json"); ;
        }
    }
}
