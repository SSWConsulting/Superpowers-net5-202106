using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Forecasts
{
    public class WeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastService> _logger;

        public WeatherForecastService(ILogger<WeatherForecastService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<WeatherForecast> GetForecasts(int count)
        {
            _logger.LogInformation($"Getting {count} forecasts.");

            var rng = new Random();

            var forecasts = Enumerable.Range(1, count).Select(index =>
                new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                });

            return forecasts;
        }
    }
}
