using System;

namespace NoteTaker.DAL.Entities
{
    public class Note : Entity
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public Note()
        {
            
        }

        public Note(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public void Update(string title, string content)
        {
            Title = title;
            Content = content;
            Modified = DateTime.UtcNow;
        }
    }
}
