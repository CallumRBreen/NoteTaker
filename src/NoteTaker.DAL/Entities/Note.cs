using System;

namespace NoteTaker.DAL.Entities
{
    public class Note : Entity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        internal Note()
        {
            
        }

        public Note(string title, string content, Guid userId)
        {
            Title = title;
            Content = content;
            UserId = userId;
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
