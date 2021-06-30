using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Net5Superpowers.WebUI.Data;
using Respawn;
using Xunit;

namespace Net5Superpowers.WebUI.IntegrationTests
{
    [CollectionDefinition(nameof(TestFixture))]
    public class TestFixtureCollection : ICollectionFixture<TestFixture> { }

    public class TestFixture : IAsyncLifetime
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Checkpoint _checkpoint;

        public TestFixture()
        {
            Factory = new CustomWebApplicationFactory();

            _configuration = Factory.Services.GetRequiredService<IConfiguration>();

            _scopeFactory = Factory.Services.GetRequiredService<IServiceScopeFactory>();

            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };
        }

        public CustomWebApplicationFactory Factory { get; }

        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
        }

        public StringContent GetRequestContent(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }

        public async Task<T> GetResponseContent<T>(HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<T>(stringResponse);

            return result;
        }

        public Task DisposeAsync()
        {
            Factory?.Dispose();

            return Task.CompletedTask;
        }
    }
}
