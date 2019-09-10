using System;
using NoteTaker.Core.Services.Interfaces;

namespace NoteTaker.Core.Services.Implementations
{
    public class BCryptPasswordHashingService : IPasswordHashingService
    {
        public string GetPasswordHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be null or contain only spaces", nameof(password));
            }

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentNullException(nameof(passwordHash));

            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
