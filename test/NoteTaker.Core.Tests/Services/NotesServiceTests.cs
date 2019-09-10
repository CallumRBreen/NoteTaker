using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteTaker.Core.Services.Implementations;
using NoteTaker.Core.Tests.Helpers;
using NoteTaker.DAL;
using NoteTaker.DAL.Entities;
using Xunit;

namespace NoteTaker.Core.Tests.Services
{
    public class NotesServiceTests
    {
        [Fact]
        public async Task Get_Note_Successfully()
        {
            var noteId = Guid.NewGuid();

            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Get_Note_Successfully));

            using (var context = new NoteTakerContext(options))
            {
                context.Add(new Note
                {
                    Id = noteId,
                    Title = "Apples",
                    Content = "Oranges"
                });

                context.SaveChanges();
            };

            using (var context = new NoteTakerContext(options))
            {
                var service = new NotesService(context);

                var note = await service.GetNoteAsync(noteId.ToString());

                Assert.NotNull(note);
                Assert.Equal(noteId.ToString(), note.Id);
            };
        }

        [Fact]
        public async Task Create_Note_Successfully()
        {
            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Create_Note_Successfully));

            using (var context = new NoteTakerContext(options))
            {
                var service = new NotesService(context);

                var note = await service.CreateNoteAsync("Apples", "Oranges");

                Assert.NotNull(note);
            };
        }

        [Fact]
        public async Task Update_Note_Successfully()
        {
            var noteId = Guid.NewGuid();

            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Update_Note_Successfully));

            using (var context = new NoteTakerContext(options))
            {
                context.Add(new Note
                {
                    Id = noteId,
                    Title = "Apples",
                    Content = "Oranges"
                });

                context.SaveChanges();
            };

            using (var context = new NoteTakerContext(options))
            {
                var service = new NotesService(context);

                var updatedNote = await service.UpdateNoteAsync(noteId.ToString(), "New Title", "New Content");

                Assert.NotNull(updatedNote);
                Assert.Equal("New Title",updatedNote.Title);
                Assert.Equal("New Content", updatedNote.Content);
            };
        }

        [Fact]
        public async Task Search_Notes_Successfully()
        {
            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Search_Notes_Successfully));

            using (var context = new NoteTakerContext(options))
            {
                context.Notes.AddRange(new List<Note>
                {
                    new Note
                    {
                        Id = Guid.NewGuid(),
                        Title = "Apples",
                        Content = "Oranges",
                        Created = DateTime.Now,
                        Modified = DateTime.Now.AddDays(-7)
                    },
                    new Note
                    {
                        Id = Guid.NewGuid(),
                        Title = "Pineapples",
                        Content = "Bananas",
                        Created = DateTime.Now,
                        Modified = DateTime.Now.AddDays(-3)
                    },
                    new Note
                    {
                        Id = Guid.NewGuid(),
                        Title = "Avocado",
                        Content = "Broccoli",
                        Created = DateTime.Now,
                        Modified = DateTime.Now.AddDays(-10)
                    },
                });

                context.SaveChanges();
            };

            using (var context = new NoteTakerContext(options))
            {
                var service = new NotesService(context);

                var notes = await service.GetNotesAsync("Avocado");

                Assert.Single(notes);
                Assert.Equal("Avocado", notes.First().Title);
            };
        }

        [Fact]
        public async Task Get_Notes_OrderedByModifiedDesc_Successfully()
        {
            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Get_Notes_OrderedByModifiedDesc_Successfully));

            using (var context = new NoteTakerContext(options))
            {
                context.Notes.AddRange(new List<Note>
                {
                    new Note
                    {
                        Id = Guid.NewGuid(),
                        Title = "Apples",
                        Content = "Oranges",
                        Created = DateTime.Now,
                        Modified = DateTime.Now.AddDays(-7)
                    },
                    new Note
                    {
                        Id = Guid.NewGuid(),
                        Title = "Pineapples",
                        Content = "Bananas",
                        Created = DateTime.Now,
                        Modified = DateTime.Now.AddDays(-3)
                    },
                    new Note
                    {
                        Id = Guid.NewGuid(),
                        Title = "Avocado",
                        Content = "Broccoli",
                        Created = DateTime.Now,
                        Modified = DateTime.Now.AddDays(-10)
                    },
                });

                context.SaveChanges();
            };

            using (var context = new NoteTakerContext(options))
            {
                var service = new NotesService(context);

                var notes = await service.GetNotesAsync(string.Empty);

                Assert.Equal("Pineapples", notes[0].Title);
                Assert.Equal("Apples", notes[1].Title);
                Assert.Equal("Avocado", notes[2].Title);
            };
        }
    }
}
