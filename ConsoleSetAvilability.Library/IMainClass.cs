using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library
{
    public interface IMainClass
    {
        Task<HttpClient?> RunUpdatesAvailabilityProducts();
        Task RunUpdateCheckboxesProducts(HttpClient? client = null);
    }
}
