using System.Text.Json;

namespace Training.Core.interfaces
{
    public interface ITaipeiWeatherService
    {
        Task<JsonDocument> GetWeatherAsync();
    }
}
