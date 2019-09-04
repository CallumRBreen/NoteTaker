using System.Collections.Generic;
using System.Threading.Tasks;
using NoteTaker.Core.Models;

namespace NoteTaker.Core.Services.Interfaces
{
    public interface INotesService
    {
        Task<Note> GetNoteAsync(string id);
        Task<Note> CreateNoteAsync(string title, string content);
        Task<Note> UpdateNoteAsync(string id, string title, string content);
        Task<List<Note>> GetNotesAsync(string searchText);
    }
}
