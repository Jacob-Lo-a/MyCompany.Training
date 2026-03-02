using System;
namespace Training.Core
{

    public class SystemInfo
    {
        public string GetOSAndDateTime()
        {

            string osName = Environment.OSVersion.VersionString;

            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            return $"作業系統: {osName}, 時間: {currentTime} 測試";
        }

    }

}
