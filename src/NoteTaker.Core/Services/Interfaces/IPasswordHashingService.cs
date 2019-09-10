namespace NoteTaker.Core.Services.Interfaces
{
    public interface IPasswordHashingService
    {
        string GetPasswordHash(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
