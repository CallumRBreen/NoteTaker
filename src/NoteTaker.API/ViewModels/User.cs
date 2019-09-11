using System;

namespace NoteTaker.API.ViewModels
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
