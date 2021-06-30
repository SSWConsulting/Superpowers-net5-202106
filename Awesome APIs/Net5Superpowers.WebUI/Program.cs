using Azure.Core;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Net5Superpowers.WebUI.Data;
using System;

namespace Net5Superpowers.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    ApplicationDbContextInitialiser.Initialise(services.GetRequiredService<ApplicationDbContext>());
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occurred attempting to initialise the database.");

                    throw;
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    IConfigurationRoot builtConfig = config.Build();

                    // ATTENTION:
                    //
                    // If running the app from your local dev machine (not in Azure AppService),
                    // -> use the AzureCliCredential provider.
                    // -> This means you have to log in locally via `az login` before running the app on your local machine.
                    //
                    // If running the app from Azure AppService
                    // -> use the DefaultAzureCredential provider
                    //
                    TokenCredential cred = context.HostingEnvironment.IsAzureAppService()
                        ? new DefaultAzureCredential(false)
                        : new AzureCliCredential();

                    var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
                    var secretClient = new SecretClient(keyVaultEndpoint, cred);
                    config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
                })
                .ConfigureAppConfiguration(config =>
                {
                    var settings = config.Build();
                    var connection = settings.GetConnectionString("AppConfig");
                    config.AddAzureAppConfiguration(opt =>
                    {
                        opt.Connect(connection)
                           .UseFeatureFlags(cfg => cfg.CacheExpirationInterval = TimeSpan.FromSeconds(3));
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public static class IHostEnvironmentExtensions
    {
        public static bool IsAzureAppService(this IHostEnvironment env)
        {
            var websiteName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
            return string.IsNullOrEmpty(websiteName) is not true;
        }
    }
}
