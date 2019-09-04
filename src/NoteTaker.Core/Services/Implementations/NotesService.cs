using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteTaker.Core.Models;
using NoteTaker.Core.Services.Interfaces;
using NoteTaker.DAL;

namespace NoteTaker.Core.Services.Implementations
{
    public class NotesService : INotesService
    {
        private readonly NoteTakerContext context;

        public NotesService(NoteTakerContext context)
        {
            this.context = context;
        }

        public async Task<Note> GetNoteAsync(string id)
        {
            var note = await context.Notes.FirstOrDefaultAsync(n => n.Id == id);

            if (note == null) return null;

            return new Note(note);
        }

        public async Task<Note> CreateNoteAsync(string title, string name)
        {
            var note = new DAL.Entities.Note(title, name);

            context.Notes.Add(note);

            await context.SaveChangesAsync();

            return new Note(note);
        }

        public async Task<Note> UpdateNoteAsync(string id, string title, string content)
        {
            var note = await context.Notes.FirstOrDefaultAsync(n => n.Id == id);

            if (note == null) return null;

            note.Update(title, content);

            await context.SaveChangesAsync();

            return new Note(note);
        }

        public async Task<List<Note>> GetNotesAsync(string searchText)
        {
            var notes = context.Notes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                notes = notes.Where(n => n.Title.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)
                                         || n.Content.Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
            }

            notes = notes.OrderByDescending(n => n.Modified);

            return await notes.Select(n => new Note(n)).ToListAsync();
        }
    }
}
