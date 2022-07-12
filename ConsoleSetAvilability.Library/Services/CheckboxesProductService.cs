using ConsoleSetAvilability.Library.Logic.Interfaces;
using ConsoleSetAvilability.Library.Models;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using ConsoleSetAvilability.Library.Models.JsonModels;
using ConsoleSetAvilability.Library.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Services
{
    public class CheckboxesProductService : ICheckboxesProductService
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<ICheckboxesProductService> _logger;
        private readonly IWebService _webService;
        private readonly IWebShopService _webShopService;
        private readonly HttpClient _shopClient;

        public CheckboxesProductService(
            IOptions<AppSettings> appSettings,
            ILogger<ICheckboxesProductService> logger,
            IWebService webService,
            IWebShopService webShopService)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _webService = webService;
            _webShopService = webShopService;
            _shopClient = webService.GetHttpClient();
        }

        public async Task<List<ResponseReserveProduct>> SetCheckboxes(PrincipleCheckbox principle, HttpClient apiClient)
        {
            List <ResponseReserveProduct> responses = new();
            ApiShopSettings apiShopSettings = _appSettings.ApiShopData;
            string url = apiShopSettings.BaseUrlShopApi + apiShopSettings.UrlProductReserve;

            foreach (var symbolObject in principle.Symbols)
            {
                OnlineProduct product = new() { Symbol = symbolObject.Symbol };
                ShopProduct shopProduct = await _webService.GetCurrentlyShopProduct(product, apiShopSettings);
                ResponseReserveProduct responseReserve = new ResponseReserveProduct() { Symbol = product.Symbol };
                decimal priceGross = 0;
                decimal? result = GetCurrentPrice(shopProduct);
                bool changed = false;
                
                if (result != null)
                    priceGross = decimal.Parse(result.ToString());
                string message = $"Cena-principle: {principle.PriceGross}, price-shop: {priceGross}";

                if (principle.CeneoBuyNow)
                {
                    if (priceGross > principle.PriceGross 
                        && shopProduct.Stock > principle.Stock
                        && shopProduct.IsCeneoBuyNow == false)
                    {
                        HttpContent updatedParameters = CreateParameters(shopProduct, true, apiShopSettings.KeyCeneoKupTeraz);
                        responseReserve = await _webShopService.MakePostWithParameters(url, updatedParameters, _shopClient, responseReserve);
                        changed = true;
                        AddOnlyNegativeResponse(responses, responseReserve);
                    }

                    if (shopProduct.Stock <= principle.Stock
                        && shopProduct.IsCeneoBuyNow == true)
                    {
                        HttpContent updatedParameters = CreateParameters(shopProduct, false, apiShopSettings.KeyCeneoKupTeraz);
                        responseReserve = await _webShopService.MakePostWithParameters(url, updatedParameters, _shopClient, responseReserve);
                        changed = true;
                        AddOnlyNegativeResponse(responses, responseReserve);
                    }

                    message = $"Ilość shop: {shopProduct.Stock}, CeneoBuyNow: { principle.CeneoBuyNow}, Shop-CeneoBuyNow {shopProduct.IsCeneoBuyNow}";
                }

                if (principle.GoogleMarchent)
                {
                    if (priceGross > 0 && shopProduct.IsGoogleMerchant == false)
                    {
                        HttpContent updatedParameters = CreateParameters(shopProduct, true, apiShopSettings.KeyGoogleMerchant);
                        responseReserve = await _webShopService.MakePostWithParameters(url, updatedParameters, _shopClient, responseReserve);
                        changed = true;
                        AddOnlyNegativeResponse(responses, responseReserve);
                    }

                    if (priceGross <= 0 && shopProduct.IsGoogleMerchant == true)
                    {
                        HttpContent updatedParameters = CreateParameters(shopProduct, false, apiShopSettings.KeyGoogleMerchant);
                        responseReserve = await _webShopService.MakePostWithParameters(url, updatedParameters, _shopClient, responseReserve);
                        changed = true;
                        AddOnlyNegativeResponse(responses, responseReserve);
                    }

                    message = $"GoogleMerchant: { principle.GoogleMarchent}, Shop-GoogleMerchant: {shopProduct.IsGoogleMerchant}";
                }

                if (!changed)
                    message = "Już było ustawione." + message;

                responseReserve.Message = message;
                //_logger.LogInformation(message);
            }

            return responses;
        }

        private void AddOnlyNegativeResponse(List<ResponseReserveProduct> responses, ResponseReserveProduct responseReserve)
        {
            if (!responseReserve.IsUpdated || !responseReserve.IsSynchronize)
                responses.Add(responseReserve);
        }

        private HttpContent CreateParameters(ShopProduct shopProduct, bool checkbox, string nameField)
        {
            string chekboxValue = checkbox == true ? "on" : "off";

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "Symbol", shopProduct.Symbol },
                { nameField, chekboxValue }
            };
            string jsonData = JsonConvert.SerializeObject(parameters);

            return new StringContent(jsonData, Encoding.UTF8, "application/json"); ;
        }

        private decimal? GetCurrentPrice(ShopProduct shopProduct)
        {
            if (shopProduct.PriceGroupRebateGross != null)
                return shopProduct.PriceGroupRebateGross;

            if (shopProduct.PriceGross != null)
                return shopProduct.PriceGross;

            return null;
        }
    }
}
