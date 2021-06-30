using System.Threading.Tasks;
using Net5Superpowers.WebUI.Models;
using Xunit;

namespace Net5Superpowers.WebUI.IntegrationTests.Controllers
{
    [Collection(nameof(TestFixture))]
    public class TodoListsControllerTests
    {
        private readonly TestFixture _fixture;

        public TodoListsControllerTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetReturnsSuccessAndCorrectVm()
        {
            // Arrange
            var client = _fixture.Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/todolists");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateReturnsSuccess()
        {
            var client = _fixture.Factory.CreateClient();

            var vm = new CreateTodoListVm { Title = "Shopping List" };

            var response = await client.PostAsync("/api/todolists", _fixture.GetRequestContent(vm));

            response.EnsureSuccessStatusCode();
        }
    }
}
