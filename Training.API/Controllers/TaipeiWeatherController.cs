using Microsoft.AspNetCore.Mvc;
using Training.Core;
using Training.Core.interfaces;

namespace Training.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaipeiWeatherController : ControllerBase
    {
        private readonly ITaipeiWeatherService _weatherService;

        public TaipeiWeatherController(ITaipeiWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _weatherService.GetWeatherAsync();
            return Ok(result);
        }
    }
}
