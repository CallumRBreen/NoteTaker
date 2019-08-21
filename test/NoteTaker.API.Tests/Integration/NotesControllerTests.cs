using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NoteTaker.API.ViewModels;
using Xunit;

namespace NoteTaker.API.Tests.Integration
{
    public class NotesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public NotesControllerTests(WebApplicationFactory<Startup> factory)
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
        public async Task Get_Note_Successfully()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync($"api/notes/{Guid.NewGuid().ToString()}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Update_Note_Successfully()
        {
            var client = factory.CreateClient();
            
            var httpContent = new StringContent(JsonConvert.SerializeObject(GetUpdateNoteTestData()));

            var response = await client.PutAsJsonAsync($"api/notes/{Guid.NewGuid().ToString()}", httpContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Create_Note_Successfully()
        {
            var client = factory.CreateClient();

            var httpContent = new StringContent(JsonConvert.SerializeObject(GetCreateNoteTestData()));

            var response = await client.PostAsJsonAsync($"api/notes", httpContent);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Patch_Note_Successfully()
        {
            var client = factory.CreateClient();

            var operations = GetPatchNoteOperations();

            var httpContent = new StringContent(JsonConvert.SerializeObject(operations), Encoding.UTF8, "application/json");

            var response = await client.PatchAsync($"api/notes/{Guid.NewGuid().ToString()}", httpContent);

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

            var jsonPatchDocument =  new JsonPatchDocument<Note>(operations, new JsonContractResolver(new JsonMediaTypeFormatter()));

            return jsonPatchDocument;
        }

        private static UpdateNote GetUpdateNoteTestData() => new UpdateNote {Title = "New Note", Content = "New Note Content"};

        private static CreateNote GetCreateNoteTestData() => new CreateNote { Title = "New Note", Content = "New Note Content" };
    }
}
