using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;
using NoteTaker.API.Tests.TestHelpers;
using NoteTaker.API.ViewModels;
using Xunit;

namespace NoteTaker.API.Tests.Integration
{
    public class NotesControllerTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly TestWebApplicationFactory<Startup> factory;

        public NotesControllerTests(TestWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }


        [Fact]
        public async Task Get_All_Notes_Successfully()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync("api/notes");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Search_All_Notes_Successfully()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync("api/notes?text=apples");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseModel = await response.Content.ReadAsAsync<List<Note>>();

            Assert.Equal("Apples", responseModel.First().Title);
        }

        [Fact]
        public async Task Get_Note_Successfully()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync($"api/notes/11111111-1234-4133-8c69-40ca0509be6a");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Update_Note_Successfully()
        {
            var client = factory.CreateClient();
            
            var response = await client.PutAsJsonAsync($"api/notes/22222222-4321-1234-4321-40ca0509be6a", GetUpdateNoteTestData());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Create_Note_Successfully()
        {
            var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync($"api/notes", GetCreateNoteTestData());

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Patch_Note_Successfully()
        {
            var client = factory.CreateClient();

            var operations = GetPatchNoteOperations();

            var httpContent = new StringContent(JsonConvert.SerializeObject(operations), Encoding.UTF8, "application/json");

            var response = await client.PatchAsync($"api/notes/33333333-4321-1234-4321-40ca0509be6a", httpContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseModel = await response.Content.ReadAsAsync<Note>();

            Assert.Equal("New Title", responseModel.Title);
            Assert.Equal("New Content", responseModel.Content);
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
