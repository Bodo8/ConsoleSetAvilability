using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Commons.Exceptions
{
    public class ConnectionFailException : Exception
    {
        public ConnectionFailException() : base() { }

        public ConnectionFailException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
