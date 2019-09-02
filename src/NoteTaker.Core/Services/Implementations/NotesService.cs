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

        public Note GetNote(string id)
        {
            throw new System.NotImplementedException();
        }

        public Note CreateNote(string title, string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Note>> GetNotesAsync(string searchText)
        {
            var notes = context.Notes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                notes = notes.Where(n => n.Title.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)
                                         || n.Content.Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
            }

            return await notes.Select(n => new Note(n)).ToListAsync();
        }
    }
}
