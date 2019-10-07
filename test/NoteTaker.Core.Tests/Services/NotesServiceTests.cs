using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NoteTaker.Core.Services.Implementations;
using NoteTaker.Core.Tests.Helpers;
using NoteTaker.DAL;
using NoteTaker.DAL.Entities;
using Xunit;

namespace NoteTaker.Core.Tests.Services
{
    public class NotesServiceTests
    {
        private readonly Mock<IHttpContextAccessor> httpContextAccessor;
        private readonly User user;

        public NotesServiceTests()
        {
            httpContextAccessor = new Mock<IHttpContextAccessor>();

            user = new User(Guid.NewGuid());

            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(new TestPrincipal(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()))).Verifiable();
        }

        [Fact]
        public async Task Get_Note_Successfully()
        {
            var noteId = Guid.NewGuid();

            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Get_Note_Successfully));

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                context.Add(new Note
                {
                    Id = noteId,
                    Title = "Apples",
                    Content = "Oranges",
                    User = user
                });

                context.SaveChanges();
            };

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                var service = new NotesService(context, httpContextAccessor.Object);

                var note = await service.GetNoteAsync(noteId.ToString());

                note.Should().NotBeNull();
                noteId.ToString().Should().BeEquivalentTo(note.Id);
            };
        }

        [Fact]
        public async Task Create_Note_Successfully()
        {
            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Create_Note_Successfully));

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                var service = new NotesService(context, httpContextAccessor.Object);

                var note = await service.CreateNoteAsync("Apples", "Oranges");

                note.Should().NotBeNull();

                httpContextAccessor.Verify(x => x.HttpContext.User, Times.Once);
            };
        }

        [Fact]
        public async Task Update_Note_Successfully()
        {
            var noteId = Guid.NewGuid();

            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Update_Note_Successfully));

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                context.Add(new Note
                {
                    Id = noteId,
                    Title = "Apples",
                    Content = "Oranges",
                    User = user
                });

                context.SaveChanges();
            };

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                var service = new NotesService(context, httpContextAccessor.Object);

                var updatedNote = await service.UpdateNoteAsync(noteId.ToString(), "New Title", "New Content");

                updatedNote.Should().NotBeNull();
                updatedNote.Title.Should().BeEquivalentTo("New Title");
                updatedNote.Content.Should().BeEquivalentTo("New Content");
            };
        }

        [Fact]
        public async Task Search_Notes_Successfully()
        {
            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Search_Notes_Successfully));

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                context.Notes.AddRange(new List<Note>
                {
                    new Note
                    {
                        Id = Guid.NewGuid(),
                        Title = "Apples",
                        Content = "Oranges",
                        Created = DateTime.Now,
                        Modified = DateTime.Now.AddDays(-7),
                        User = user
                    },
                    new Note
                    {
                        Id = Guid.NewGuid(),
                        Title = "Pineapples",
                        Content = "Bananas",
                        Created = DateTime.Now,
                        Modified = DateTime.Now.AddDays(-3),
                        User = user
                    },
                    new Note
                    {
                        Id = Guid.NewGuid(),
                        Title = "Avocado",
                        Content = "Broccoli",
                        Created = DateTime.Now,
                        Modified = DateTime.Now.AddDays(-10),
                        User = user
                    },
                });

                context.SaveChanges();
            };

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                var service = new NotesService(context, httpContextAccessor.Object);

                var notes = await service.GetNotesAsync("Avocado");

                notes.Should().ContainSingle();
                notes.First().Title.Should().BeEquivalentTo("Avocado");
            };
        }

        [Fact]
        public async Task Get_Notes_OrderedByModifiedDesc_Successfully()
        {
            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Get_Notes_OrderedByModifiedDesc_Successfully));

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                context.Notes.AddRange(new List<Note>
                {
                    new Note
                    {
                        Id = Guid.NewGuid(),
                        Title = "Apples",
                        Content = "Oranges",
                        Created = DateTime.Now,
                        Modified = DateTime.Now.AddDays(-7),
                        User = user
                    },
                    new Note
                    {
                        Id = Guid.NewGuid(),
                        Title = "Pineapples",
                        Content = "Bananas",
                        Created = DateTime.Now,
                        Modified = DateTime.Now.AddDays(-3),
                        User = user
                    },
                    new Note
                    {
                        Id = Guid.NewGuid(),
                        Title = "Avocado",
                        Content = "Broccoli",
                        Created = DateTime.Now,
                        Modified = DateTime.Now.AddDays(-10),
                        User = user
                    },
                });

                context.SaveChanges();
            };

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                var service = new NotesService(context, httpContextAccessor.Object);

                var notes = await service.GetNotesAsync(string.Empty);

                notes[0].Title.Should().BeEquivalentTo("Pineapples");
                notes[1].Title.Should().BeEquivalentTo("Apples");
                notes[2].Title.Should().BeEquivalentTo("Avocado");
            };
        }

        [Fact]
        public async Task Delete_Note_Successfully()
        {
            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Delete_Note_Successfully));

            var noteToDelete = Guid.NewGuid();

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                context.Notes.Add(new Note
                {
                        Id = noteToDelete,
                        Title = "Apples",
                        Content = "Oranges",
                        Created = DateTime.Now,
                        Modified = DateTime.Now.AddDays(-7),
                        User = user
                });

                context.SaveChanges();
            };

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                var service = new NotesService(context, httpContextAccessor.Object);

                await service.DeleteNoteAsync(noteToDelete.ToString());

                context.Notes.FirstOrDefault(x => x.Id == noteToDelete).Should().BeNull();
            };
        }
    }
}
