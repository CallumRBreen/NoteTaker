using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NoteTaker.API;
using NoteTaker.API.ViewModels;
using NoteTaker.IntegrationTests.TestHelpers;
using Xunit;

namespace NoteTaker.IntegrationTests
{
    public class ViewModelValidationTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;

        public ViewModelValidationTests(TestWebApplicationFactory<Startup> factory)
        {
            this.client = factory.CreateClient().AddTestJwtHeader();
        }

        [Fact]
        public async Task FluentValidation_Automatically_Applied()
        {
            var response = await client.PostAsJsonAsync("api/notes", GetCreateNoteTestData());

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

        private static CreateNote GetCreateNoteTestData() => new CreateNote { Title = null, Content = null };
    }
}
