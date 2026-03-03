using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
namespace Training.Core
{

    public class Class1
    {
        public string GetOSAndDateTime()
        {

            string osName = Environment.OSVersion.VersionString;

            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            return $"作業系統: {osName}, 時間: {currentTime}";
        }

        
    }

    
}
    
