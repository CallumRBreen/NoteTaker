namespace NoteTaker.Core.Models
{
    public class AuthenticatedUser : User
    {
        public string Token { get; set; }

        internal AuthenticatedUser()
        {
            
        }
        public AuthenticatedUser(DAL.Entities.User user, string token)
        {
            Id = user.Id.ToString();
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Created = user.Created;
            Modified = user.Modified;
            Token = token;
        }
    }
}
