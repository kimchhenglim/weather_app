
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;
using WeatherApplication.Repositories;
using WeatherApplication.Services;

namespace WeatherApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient();
            builder.Services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
            });
            // Rate limiting
            builder.Services.AddRateLimiter(o =>
            {
                o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                // Simple fixed window limiter
                o.AddFixedWindowLimiter("request-policy", opt =>
                {
                    opt.PermitLimit = 5;                   // 5 requests
                    opt.Window = TimeSpan.FromMinutes(1);  // per minute
                    opt.QueueLimit = 0;                    // reject immediately
                });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton<WeatherForecastRepository>();

            // Redis as IDistributedCache
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                options.InstanceName = "WeatherApp:";
            });

            //builder.Services.AddHttpClient<WeatherForecastRepository>(client =>
            //{
            //    client.BaseAddress = new Uri(builder.Configuration["Weather:ApiKey"] ?? "");
            //});

            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseRateLimiter(); // Rate Limiter

            app.MapControllers();

            app.Run();
        }
    }
}
