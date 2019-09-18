using System;
using System.Collections.Generic;

namespace NoteTaker.DAL.Entities
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Note> Notes { get; set; }

        internal User()
        {
            
        }

        public User(Guid id)
        {
            Id = id;
        }

        public User(string username, string firstName, string lastName, string passwordHash)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            PasswordHash = passwordHash;
        }
    }
}
