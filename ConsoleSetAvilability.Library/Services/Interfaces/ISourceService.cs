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
    public interface ISourceService
    {
        Task<OnlineProductsObject?> GetApiProductContent(HttpClient apiclient, string urlWebsite);
        HttpClient GetHttpClientApiAuthorizeSource(ApiProductSettings apiProductSettings);
        Task<T>? GetApiContent<T>(HttpClient apiClient, string url);
    }
}
