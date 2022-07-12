using ConsoleSetAvilability.Library.Models;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using ConsoleSetAvilability.Library.Models.JsonModels;
using ConsoleSetAvilability.Library.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library
{
    public class MainClass : IMainClass
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<IMainClass> _logger;
        private readonly ISourceService _sourceService;
        private readonly IUpdateProductsByApiService _updateProductsByApiService;
        private readonly ICheckboxesProductService _checkboxesProductService;
        private readonly ApiProductSettings? _apiProductSettings;
        private readonly ApiShopSettings? _apiShopSettings;

        public MainClass(
            IOptions<AppSettings> appSettings,
            ILogger<IMainClass> logger,
            ISourceService sourceService,
            IUpdateProductsByApiService updateProductsByApiService,
            ICheckboxesProductService checkboxesProductService)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _sourceService = sourceService;
            _updateProductsByApiService = updateProductsByApiService;
            _checkboxesProductService = checkboxesProductService;
            _apiProductSettings = _appSettings?.ApiProductData;
            _apiShopSettings = _appSettings?.ApiShopData;

        }

        public async Task<HttpClient?> RunUpdatesAvailabilityProducts()
        {
            if (_appSettings != null && _appSettings.RunCheckAvailability)
            {
                try
                {
                    if (_apiProductSettings == null || _apiShopSettings == null)
                        throw new ArgumentNullException(nameof(_apiProductSettings));

                    HttpClient apiClient = _sourceService.GetHttpClientApiAuthorizeSource(_apiProductSettings);
                    string urlWebsite = _apiProductSettings.BaseUrl + _apiProductSettings.UrlWebsites;

                    WebsitesObject? websitesObject = await _sourceService.GetApiContent<WebsitesObject>(apiClient, urlWebsite);
                    List<Website>? websiteToProcess = websitesObject?.Websites.Where(w => w.CheckAvailability == true).ToList();
                    List<ResponseReserveProduct> responsesReserve = new List<ResponseReserveProduct>();

                    string urlProduct = _apiProductSettings.BaseUrl + _apiProductSettings.UrlProduct;

                    if (websiteToProcess != null)
                    {
                        foreach (Website website in websiteToProcess)
                        {
                            OnlineProductsObject? onlineProductsObject = await _sourceService.GetApiProductContent(apiClient, urlProduct + website.Id);
                            List<ResponseReserveProduct> responses = await _updateProductsByApiService.UpdateProducts(onlineProductsObject, website, _appSettings, apiClient);
                            responsesReserve.AddRange(responses);
                        }
                    }
                    else
                        _logger.LogWarning($"Brak stron do sprawdzania");

                    ShowLogWithNegativeResponse(responsesReserve);
                    _logger.LogInformation($"Aktualizacja dostępności productów zakończona");

                    return apiClient;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Błąd w klasie głównej MainClass - RunUpdatesAvailabilityProducts", ex.Message);
                } 
            }

            return null;
        }

        public async Task RunUpdateCheckboxesProducts(HttpClient? apiClient = null)
        {
            if (_appSettings != null && _appSettings.RunCheckCheckboxes)
            {
                try
                {
                    if (_apiProductSettings == null || _apiShopSettings == null)
                        throw new ArgumentNullException(nameof(_apiProductSettings));

                    if (apiClient == null)
                        apiClient = _sourceService.GetHttpClientApiAuthorizeSource(_apiProductSettings);

                    string urlPrinciple = _apiProductSettings.BaseUrl + _apiProductSettings.UrlPrinciple;
                    PrinciplesObject? principlesObject = await _sourceService.GetApiContent<PrinciplesObject>(apiClient, urlPrinciple);
                    List<PrincipleCheckbox> principles = principlesObject == null ? new List<PrincipleCheckbox>() : principlesObject.PrincipleCheckboxes;
                    List<ResponseReserveProduct> responses = new List<ResponseReserveProduct>();

                    foreach (var principle in principles)
                    {
                        var responseCheckboxes = await _checkboxesProductService.SetCheckboxes(principle, apiClient);
                        responses.AddRange(responseCheckboxes);
                    }

                    ShowLogWithNegativeResponse(responses);
                    _logger.LogInformation($"Aktualizacja Checkboxes products zakończona");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Błąd w klasie głównej MainClass - UpdateCheckboxes", ex.Message);
                }
            }
        }

        private void ShowLogWithNegativeResponse(List<ResponseReserveProduct> responsesReserve)
        {
            responsesReserve.ForEach(p =>
            {
                string responseString = JsonConvert.SerializeObject(p);
                _logger.LogWarning($"Nie powiodła się synchronizacja dla {responsesReserve.Count} productów: {responseString}");
            });
        }
    }
}
