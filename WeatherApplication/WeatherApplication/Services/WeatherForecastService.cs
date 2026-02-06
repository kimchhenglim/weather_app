using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using WeatherApplication.Dtos.Outgoing;
using WeatherApplication.Repositories;

namespace WeatherApplication.Services
{
    public class WeatherForecastService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<WeatherForecastService> _logger;
        private readonly WeatherForecastRepository _weatherForecastRepository;
        private readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(30);
        private static string CacheKey(string city) => $"cities:{city}";

        public WeatherForecastService(IDistributedCache cache, WeatherForecastRepository weatherForecastRepository, ILogger<WeatherForecastService> logger)
        {
            _cache = cache;
            _logger = logger;
            _weatherForecastRepository = weatherForecastRepository;
        }

        public async Task<WeatherForecastResponse?> GetWeatherAsync(string city)
        {
            string key = CacheKey(city);

            // Redis - Check cache
            string? cachedJson = await _cache.GetStringAsync(key);

            if (cachedJson != null) {
                _logger.LogInformation("Redis CACHE HIT for {Key}", key);
                return JsonConvert.DeserializeObject<WeatherForecastResponse?>(cachedJson);
            }

            _logger.LogInformation("Redis CACHE MISS for {Key}", key);

            // Fetch DB
            WeatherForecastResponse? dto = await _weatherForecastRepository.GetByCityAsync(city);

            if (dto == null) return null;

            // Redis - Save cache
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(dto), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheTtl });
            _logger.LogInformation("Cached {Key} in Redis (TTL={Ttl}s)", key, CacheTtl.TotalSeconds);

            return dto;
        }
    }
}
