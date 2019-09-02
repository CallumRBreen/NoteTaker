using System.Collections.Generic;
using System.Threading.Tasks;
using NoteTaker.Core.Models;

namespace NoteTaker.Core.Services.Interfaces
{
    public interface INotesService
    {
        Note GetNote(string id);
        Note CreateNote(string title, string name);
        Task<List<Note>> GetNotesAsync(string searchText);
    }
}
