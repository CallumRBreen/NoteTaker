using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NoteTaker.API.Controllers;
using NoteTaker.API.ViewModels;
using NoteTaker.Core.Services.Interfaces;
using Xunit;
using Note = NoteTaker.Core.Models.Note;

namespace NoteTaker.API.Tests.Unit
{
    public class NotesControllerTests
    {
        private readonly NotesController controller;
        private readonly Mock<INotesService> noteService;

        public NotesControllerTests()
        {
            noteService = new Mock<INotesService>();
            controller = new NotesController(new NullLogger<NotesController>(), noteService.Object);
        }

        [Fact]
        public async Task Searches_Notes_Successfully()
        {
            var queryNote = new QueryNote
            {
                Text = "Apples"
            };

            noteService.Setup(x => x.GetNotesAsync(It.Is<string>(y => y.Equals("Apples")))).ReturnsAsync(new List<Note>
            {
                new Note()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Apples"
                }
            }).Verifiable();

            var result = (await controller.Get(queryNote)).Result as OkObjectResult;

            var notes = (List<ViewModels.Note>)result?.Value;

            Assert.Equal("Apples", notes?.First().Title);

            noteService.Verify();
        }

        [Fact]
        public async Task Get_Note_Successfully()
        {
            var noteId = Guid.NewGuid().ToString();

            noteService.Setup(x => x.GetNoteAsync(noteId)).ReturnsAsync(new Note
            {
                Id = noteId
            }).Verifiable();

            var result = (await controller.Get(noteId)).Result as OkObjectResult;

            var note = (ViewModels.Note)result?.Value;

            Assert.Equal(noteId, note?.Id);

            noteService.Verify();
        }

        [Fact]
        public async Task Get_Note_Returns_NotFound()
        {
            var noteId = Guid.NewGuid().ToString();

            noteService.Setup(x => x.GetNoteAsync(noteId)).ReturnsAsync(() => null).Verifiable();

            var result = (await controller.Get(noteId)).Result;

            Assert.True(result is NotFoundResult);

            noteService.Verify();
        }

        [Fact]
        public async Task Update_Note_Successfully()
        {
            var noteId = Guid.NewGuid().ToString();

            var updateNote = new UpdateNote
            {
                Title = "Apples",
                Content = "Oranges"
            };

            noteService.Setup(x => x.UpdateNoteAsync(noteId, updateNote.Title, updateNote.Content)).ReturnsAsync(
                new Note
                {
                    Id = noteId,
                    Title = "Apples",
                    Content = "Oranges",
                    Modified = DateTime.Now,
                    Created = DateTime.Now.AddDays(-7)
                }).Verifiable();

            var result = (await controller.Update(noteId, updateNote)).Result as OkObjectResult;

            var updatedNote = (ViewModels.Note)result?.Value;

            Assert.NotNull(updatedNote);

            noteService.Verify();
        }

        [Fact]
        public async Task Update_Note_Returns_Not_Found()
        {
            var noteId = Guid.NewGuid().ToString();

            var updateNote = new UpdateNote
            {
                Title = "Apples",
                Content = "Oranges"
            };

            noteService.Setup(x => x.UpdateNoteAsync(noteId, updateNote.Title, updateNote.Content)).ReturnsAsync(() => null).Verifiable();

            var result = (await controller.Update(noteId, updateNote)).Result;

            Assert.True(result is NotFoundResult);

            noteService.Verify();
        }

        [Fact]
        public async Task Create_Note_Successfully()
        {
            var createNote = new CreateNote
            {
                Title = "Apples",
                Content = "Oranges"
            };

            noteService.Setup(x => x.CreateNoteAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Note
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Apples",
                Content = "Oranges",
                Modified = DateTime.Now,
                Created = DateTime.Now
            });

            var result = (await controller.Create(createNote)).Result as CreatedResult;

            var createdNote = (ViewModels.Note)result?.Value;

            Assert.NotNull(createdNote);

            noteService.Verify();
        }

        [Fact]
        public async Task Patch_Note_Successfully()
        {
            var noteId = Guid.NewGuid().ToString();
            var patchOperations = GetPatchNoteOperations();

            noteService.Setup(x => x.GetNoteAsync(noteId)).ReturnsAsync(new Note
            {
                Id = noteId,
                Title = "Apples",
                Content = "Oranges"
            }).Verifiable();

            noteService.Setup(x => x.UpdateNoteAsync(noteId, It.Is<string>(y => y.Equals("New Title")),
                It.Is<string>(y => y.Equals("New Content")))).ReturnsAsync(new Note
            {
                    Id = noteId,
                    Title = "Apples",
                    Content = "Oranges"
            });

            var result = (await controller.Patch(noteId, patchOperations)).Result as OkObjectResult;

            var patchedNote = (ViewModels.Note)result?.Value;

            Assert.NotNull(patchedNote);

            noteService.Verify();
        }

        [Fact]
        public async Task Patch_Note_Returns_Not_Found()
        {
            var noteId = Guid.NewGuid().ToString();
            var patchOperations = GetPatchNoteOperations();

            noteService.Setup(x => x.GetNoteAsync(noteId)).ReturnsAsync(() => null).Verifiable();

            var result = (await controller.Patch(noteId, patchOperations)).Result;

            Assert.True(result is NotFoundResult);

            noteService.Verify();
        }

        private static JsonPatchDocument<ViewModels.Note> GetPatchNoteOperations()
        {
            var operations = new List<Operation<ViewModels.Note>>()
            {
                new Operation<ViewModels.Note>()
                {
                    op = "replace",
                    path = "/title",
                    value = "New Title"
                },
                new Operation<ViewModels.Note>()
                {
                    op = "replace",
                    path = "/content",
                    value = "New Content"
                }
            };

            return new JsonPatchDocument<ViewModels.Note>(operations, new JsonContractResolver(new JsonMediaTypeFormatter()));
        }
    }
}
