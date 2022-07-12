using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Commons.Exceptions
{
    public class RequestNotSuccessfulException : Exception
    {
        public RequestNotSuccessfulException(string message)
            : base(message)
        {
        }
    }
}
