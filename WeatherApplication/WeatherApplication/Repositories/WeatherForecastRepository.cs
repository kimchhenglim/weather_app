using Newtonsoft.Json;
using WeatherApplication.Dtos.External;
using WeatherApplication.Dtos.Outgoing;

namespace WeatherApplication.Repositories
{
    public sealed class WeatherForecastRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WeatherForecastRepository> _logger;
        public WeatherForecastRepository(IConfiguration configuration, ILogger<WeatherForecastRepository> logger) 
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration["Weather:BaseURL"] ?? "")
            };
        }
        public async Task<WeatherForecastResponse?>GetByCityAsync(string city)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{city}/today?key={_configuration["Weather:ApiKey"]}&include=days");

                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                WeatherVisualCrossingResponse? obj = JsonConvert.DeserializeObject<WeatherVisualCrossingResponse>(json);
                VisualCrossingDay? days = obj!.Days!.FirstOrDefault();

                WeatherForecastResponse weatherForecastDto = new WeatherForecastResponse();

                if (days != null) 
                {
                    weatherForecastDto.Date = days.Datetime;
                    weatherForecastDto.TempMax = days.Tempmax;
                    weatherForecastDto.TempMin = days.Tempmin;
                    weatherForecastDto.Temp = days.Temp;
                    weatherForecastDto.FeelLikeMax = days.FeelLikeMax;
                    weatherForecastDto.FeelLikeMin = days.FeelLikeMin;
                    weatherForecastDto.FeelLike = days.FeelLike;
                }

                return weatherForecastDto;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API network error.");

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem.");

                return null;
            }
        }
    }
}
