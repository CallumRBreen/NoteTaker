namespace NoteTaker.API.ViewModels
{
    public class CreateUser
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public Core.Models.CreateUser ToCreateUser()
        {
            return new Core.Models.CreateUser()
            {
                Username = Username,
                FirstName = FirstName,
                LastName = LastName,
                Password = Password
            };
        }
    }
}
