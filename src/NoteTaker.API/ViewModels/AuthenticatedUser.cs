namespace NoteTaker.API.ViewModels
{
    public class AuthenticatedUser : User
    {
        public string Token { get; set; }

        public AuthenticatedUser()
        {

        }

        public AuthenticatedUser(Core.Models.AuthenticatedUser user)
        {
            Id = user.Id;
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Created = user.Created;
            Modified = user.Modified;
            Token = user.Token;
        }
    }
}
