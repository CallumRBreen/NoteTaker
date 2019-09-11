using System;

namespace NoteTaker.Core.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        internal User()
        {

        }

        public User(DAL.Entities.User user)
        {
            Id = user.Id.ToString();
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Created = user.Created;
            Modified = user.Modified;
        }
    }
}
