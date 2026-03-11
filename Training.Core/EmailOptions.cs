using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core
{
    public class EmailOptions
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
