using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Models.ConfigurationModels
{
    public class EmailSettings
    {
        public string SendEmailTo { get; set; }
        public string EmailSubject { get; set; }
        public string SmtpServer { get; set; }
        public string EmailAddress { get; set; }
        public string EmailPassword { get; set; }
    }
}
