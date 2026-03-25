using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core.interfaces
{
    public interface ITaipeiWeatherService
    {
        Task<string> GetWeatherAsync();
    }
}
