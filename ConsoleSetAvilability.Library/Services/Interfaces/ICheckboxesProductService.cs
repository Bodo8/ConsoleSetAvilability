using ConsoleSetAvilability.Library.Models;
using ConsoleSetAvilability.Library.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Services.Interfaces
{
    public interface ICheckboxesProductService
    {
        Task<List<ResponseReserveProduct>> SetCheckboxes(PrincipleCheckbox principle, HttpClient apiClient);
    }
}
