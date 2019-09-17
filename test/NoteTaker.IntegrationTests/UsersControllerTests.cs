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
        private readonly HttpClient client;

        public UsersControllerTests(TestWebApplicationFactory<Startup> factory)
        {
            this.client = factory.CreateClient();
        }

        [Fact]
        public async Task Authenticate_User_Successfully()
        {
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
            var response = await client.PostAsJsonAsync("api/users", new CreateUser
            {
                Username = "JohnAdams",
                FirstName = "John",
                LastName = "Adams",
                Password = "Apples"
            });

            response.EnsureSuccessStatusCode();

            var responseModel = await response.Content.ReadAsAsync<User>();

            responseModel.Id.Should().NotBeNullOrWhiteSpace();
        }
    }
}
