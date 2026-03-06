using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core
{
    public class WeatherService
    {
        private readonly IWeatherService _weatherService;

        public WeatherService(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public string CheckWeather()
        {
            int temp = _weatherService.GetTemp();

            if (temp == 25)
            {
                return "天氣晴朗";
            }

            return "天氣未知";
        }
    }
}
