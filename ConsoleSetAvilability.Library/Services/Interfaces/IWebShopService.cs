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
    public interface IWebShopService
    {
        Task<ResponseReserveProduct> MakePostWithParameters(
            string url,
            HttpContent? updatedParameters,
            HttpClient shopClient,
            ResponseReserveProduct responseReserve);
        HttpContent? CreateParameters(OnlineProduct product, string availabilityString, List<RangeName> rangeNames);
    }
}
