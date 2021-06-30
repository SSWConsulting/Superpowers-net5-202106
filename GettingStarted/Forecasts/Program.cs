using System;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Forecasts
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var configuration = host.Services.GetRequiredService<IConfiguration>();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            var unit = configuration.GetValue<TemperatureUnit>("Unit");

            Parser.Default.ParseArguments<Options>(args).WithParsed(options =>
            {
                Console.WriteLine("Forecasts 1.0.0");
                Console.WriteLine("Copyright (C) 2021 Forecasts");
                Console.WriteLine();

                logger.LogDebug($"Unit: {unit}, Count: {options.Count}");

                var service = host.Services.GetRequiredService<WeatherForecastService>();

                var forecasts = service.GetForecasts(options.Count);

                Console.WriteLine("\nDate\t\tTemp\tSummary");
                Console.WriteLine("----------------------------------");
                foreach (var forecast in forecasts)
                {
                    var date = forecast.Date.ToShortDateString();
                    var temp = (unit == TemperatureUnit.Celsius ? forecast.TemperatureC : forecast.TemperatureF);

                    Console.WriteLine($"{date}\t{temp}\t{forecast.Summary}");
                }
            });
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddConsoleServices();
                });
    }
}
