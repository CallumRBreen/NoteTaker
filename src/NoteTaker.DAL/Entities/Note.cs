using System;

namespace NoteTaker.DAL.Entities
{
    public class Note : Entity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
