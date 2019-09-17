using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;
using NoteTaker.API;
using NoteTaker.API.ViewModels;
using NoteTaker.IntegrationTests.TestHelpers;
using Xunit;
using Note = NoteTaker.API.ViewModels.Note;

namespace NoteTaker.IntegrationTests
{
    public class NotesControllerTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;

        public NotesControllerTests(TestWebApplicationFactory<Startup> factory)
        {
            
            this.client = factory.CreateClient().AddTestJwtHeader();
        }

        [Fact]
        public async Task Get_All_Notes_Successfully()
        {
            var response = await client.GetAsync("api/notes");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Search_All_Notes_Successfully()
        {
            var response = await client.GetAsync("api/notes?text=apples");

            response.EnsureSuccessStatusCode();

            var responseModel = await response.Content.ReadAsAsync<List<Note>>();

            responseModel.First().Title.Should().BeEquivalentTo("Apples");
        }

        [Fact]
        public async Task Get_Note_Successfully()
        {
            var response = await client.GetAsync($"api/notes/11111111-1234-4133-8c69-40ca0509be6a");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_Note_Successfully()
        {
            var response = await client.PutAsJsonAsync($"api/notes/22222222-4321-1234-4321-40ca0509be6a", GetUpdateNoteTestData());

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_Note_Successfully()
        {
            var response = await client.PostAsJsonAsync($"api/notes", GetCreateNoteTestData());

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Patch_Note_Successfully()
        {
            var operations = GetPatchNoteOperations();

            var httpContent = new StringContent(JsonConvert.SerializeObject(operations), Encoding.UTF8, "application/json");

            var response = await client.PatchAsync($"api/notes/33333333-4321-1234-4321-40ca0509be6a", httpContent);

            response.EnsureSuccessStatusCode();

            var responseModel = await response.Content.ReadAsAsync<Note>();

            responseModel.Title.Should().BeEquivalentTo("New Title");
            responseModel.Content.Should().BeEquivalentTo("New Content");
        }

        private static JsonPatchDocument<Note> GetPatchNoteOperations()
        {
            var operations = new List<Operation<Note>>()
            {
                new Operation<Note>()
                {
                    op = "replace",
                    path = "/title",
                    value = "New Title"
                },
                new Operation<Note>()
                {
                    op = "replace",
                    path = "/content",
                    value = "New Content"
                }
            };

            return new JsonPatchDocument<Note>(operations, new JsonContractResolver(new JsonMediaTypeFormatter()));
        }

        private static UpdateNote GetUpdateNoteTestData() => new UpdateNote { Title = "New Note", Content = "New Note Content" };

        private static CreateNote GetCreateNoteTestData() => new CreateNote { Title = "New Note", Content = "New Note Content" };
    }
}
