using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleSetAvilability.Library.Models;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using ConsoleSetAvilability.Library.Models.JsonModels;

namespace ConsoleSetAvilability.Library.Services.Interfaces
{
    public interface INotificationsService
    {
        Task<ResultNotifications> SendNotifications(List<ResponseReserveProduct> updatedProducts, AppSettings settings, string websiteName);
    }
}
