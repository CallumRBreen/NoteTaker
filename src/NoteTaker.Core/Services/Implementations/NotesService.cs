using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NoteTaker.Core.Models;
using NoteTaker.Core.Services.Interfaces;
using NoteTaker.DAL;
using NoteTaker.DAL.Extensions;
using Note = NoteTaker.Core.Models.Note;

namespace NoteTaker.Core.Services.Implementations
{
    public class NotesService : INotesService
    {
        private readonly NoteTakerContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public NotesService(NoteTakerContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Note> GetNoteAsync(string id)
        {
            var note = await context.Notes.FirstOrDefaultAsync(n => n.Id.ToString() == id);

            if (note == null) return null;

            return new Note(note);
        }

        public async Task<Note> CreateNoteAsync(string title, string name)
        {
            var note = new DAL.Entities.Note(title, name, httpContextAccessor.GetCurrentUserId());

            context.Notes.Add(note);

            await context.SaveChangesAsync();

            return new Note(note);
        }

        public async Task<Note> UpdateNoteAsync(string id, string title, string content)
        {
            var note = await context.Notes.FirstOrDefaultAsync(n => n.Id.ToString() == id);

            if (note == null) return null;

            note.Update(title, content);

            await context.SaveChangesAsync();

            return new Note(note);
        }

        public async Task<List<Note>> GetNotesAsync(string searchText, string orderBy)
        {
            var notes = context.Notes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                notes = notes.Where(n => n.Title.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)
                                         || n.Content.Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
            }

            notes = OrderNotes(notes, orderBy);

            return await notes.Select(n => new Note(n)).ToListAsync();
        }

        public async Task DeleteNoteAsync(string id)
        {
            var noteToDelete = await context.Notes.FirstOrDefaultAsync(n => n.Id.ToString() == id);

            if (noteToDelete == null) return;

            context.Notes.Remove(noteToDelete);

            await context.SaveChangesAsync();
        }

        private IQueryable<DAL.Entities.Note> OrderNotes(IQueryable<DAL.Entities.Note> notes, string orderBy)
        {
            var orderedNotes = notes;

            if (Enum.TryParse(orderBy, out NoteOrderBy noteOrderBy))
            {
                switch (noteOrderBy)
                {
                    case NoteOrderBy.CreatedNewest:
                        orderedNotes = notes.OrderByDescending(x => x.Created);
                        break;
                    case NoteOrderBy.CreatedOldest:
                        orderedNotes = notes.OrderBy(x => x.Created);
                        break;
                    case NoteOrderBy.ModifiedNewest:
                        orderedNotes = notes.OrderByDescending(x => x.Modified);
                        break;
                    case NoteOrderBy.ModifiedOldest:
                        orderedNotes = notes.OrderBy(x => x.Modified);
                        break;
                    case NoteOrderBy.TitleAtoZ:
                        orderedNotes = notes.OrderBy(x => x.Title);
                        break;
                    case NoteOrderBy.TitleZtoA:
                        orderedNotes = notes.OrderByDescending(x => x.Title);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                orderedNotes = notes.OrderByDescending(n => n.Modified);
            }

            return orderedNotes;
        }
    }
}
