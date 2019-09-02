using System;

namespace NoteTaker.Core.Models
{
    public class Note
    {
        public string Id { get; }
        public string Title { get; }
        public string Content { get; }
        public DateTime Created { get; }
        public DateTime Modified { get; }

        public Note(DAL.Entities.Note note)
        {
            Id = note.Id;
            Title = note.Title;
            Content = note.Content;
            Created = note.Created;
            Modified = note.Modified;
        }
    }
}
