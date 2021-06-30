using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Net5Superpowers.WebUI.Models;

namespace Net5Superpowers.WebUI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IFeatureManagerSnapshot _featureManager;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            IFeatureManagerSnapshot featureManager,
            ILogger<WeatherForecastController> logger)
        {
            _featureManager = featureManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var betaEnabled = await _featureManager.IsEnabledAsync(nameof(MyFeatureFlags.Beta));

            var rng = new Random();
            return Enumerable.Range(1, 7).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = FormatSummaries(rng, betaEnabled)
            })
            .ToArray();
        }

        private static string FormatSummaries(Random rng, bool betaEnabled)
        {
            string summary = Summaries[rng.Next(Summaries.Length)];
            return betaEnabled ? summary.ToUpper() : summary;
        }
    }
}
