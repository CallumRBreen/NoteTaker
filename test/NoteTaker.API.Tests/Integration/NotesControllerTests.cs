using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        private UpdateNote GetUpdateNoteTestData() => new UpdateNote {Title = "New Note", Content = "New Note Content"};

        private CreateNote GetCreateNoteTestData() => new CreateNote { Title = "New Note", Content = "New Note Content" };
    }
}
