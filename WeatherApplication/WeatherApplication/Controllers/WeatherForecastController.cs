using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WeatherApplication.Dtos.Outgoing;
using WeatherApplication.Services;

namespace WeatherApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherForecastService _weatherForecastService;

        public WeatherForecastController(WeatherForecastService weatherForecastService, ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _weatherForecastService = weatherForecastService;
        }

        [HttpGet(Name = "GetByCity")]
        [EnableRateLimiting("request-policy")]
        public async Task<ActionResult<WeatherForecastResponse?>> Get(string? city)
        {
            if (string.IsNullOrWhiteSpace(city)) return BadRequest("City not match");

            WeatherForecastResponse? result = await _weatherForecastService.GetWeatherAsync(city.ToUpper());

            return result;
        }
    }
}
