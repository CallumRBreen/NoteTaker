using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NoteTaker.API;
using NoteTaker.API.ViewModels;
using NoteTaker.IntegrationTests.TestHelpers;
using Xunit;

namespace NoteTaker.IntegrationTests
{
    public class ViewModelValidationTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly TestWebApplicationFactory<Startup> factory;

        public ViewModelValidationTests(TestWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task FluentValidation_Automatically_Applied()
        {
            var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync("api/notes", GetCreateNoteTestData());

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private static CreateNote GetCreateNoteTestData() => new CreateNote { Title = null, Content = null };
    }
}
