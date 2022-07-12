using ConsoleSetAvilability.Library.Models;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using ConsoleSetAvilability.Library.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Logic.Interfaces
{
    public interface IProductSettingsProcessor
    {
        Task<ResponseReserveProduct> SetAvailabilitySimpleSettings(
            List<StatusRangeMath> statuses,
            OnlineProduct product,
            AppSettings appSettings,
            HttpClient apiClient);
    }
}
