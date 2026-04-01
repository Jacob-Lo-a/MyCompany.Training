using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core
{
    
    public class Rootobject
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public Emailsettings EmailSettings { get; set; }
        public Connectionstrings ConnectionStrings { get; set; }
    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string MicrosoftAspNetCore { get; set; }
        public string MicrosoftEntityFrameworkCore { get; set; }
    }

    public class Emailsettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
    }

    public class Connectionstrings
    {
        public string BookStoreDb { get; set; }
    }

    public class SftpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RemotePath { get; set; }
    }
}
