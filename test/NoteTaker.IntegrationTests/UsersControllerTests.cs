using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NoteTaker.API;
using NoteTaker.API.ViewModels;
using NoteTaker.IntegrationTests.TestHelpers;
using Xunit;

namespace NoteTaker.IntegrationTests
{
    public class UsersControllerTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly TestWebApplicationFactory<Startup> factory;

        public UsersControllerTests(TestWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Authenticate_User_Successfully()
        {
            var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync("api/users/login", new UserLogin
            {
                Username = "JohnSmith",
                Password = "Apples"
            });

            response.EnsureSuccessStatusCode();

            var responseModel = await response.Content.ReadAsAsync<AuthenticatedUser>();

            responseModel.Token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Create_User_Successfully()
        {
            var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync("api/users", new CreateUser
            {
                Username = "JohnSmith",
                FirstName = "John",
                LastName = "Smith",
                Password = "Apples"
            });

            response.EnsureSuccessStatusCode();

            var responseModel = await response.Content.ReadAsAsync<User>();

            responseModel.Id.Should().NotBeNullOrWhiteSpace();
        }
    }
}
