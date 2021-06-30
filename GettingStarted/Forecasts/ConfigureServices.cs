using Microsoft.Extensions.DependencyInjection;

namespace Forecasts
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddConsoleServices(this IServiceCollection services)
        {
            services.AddTransient<WeatherForecastService>();

            return services;
        }
    }
}
