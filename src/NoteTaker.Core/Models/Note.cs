using System;

namespace NoteTaker.Core.Models
{
    public class Note
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public Note()
        {
            
        }

        public Note(DAL.Entities.Note note)
        {
            Id = note.Id.ToString();
            Title = note.Title;
            Content = note.Content;
            Created = note.Created;
            Modified = note.Modified;
        }
    }
}
