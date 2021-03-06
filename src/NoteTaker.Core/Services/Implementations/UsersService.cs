﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoteTaker.Core.Models;
using NoteTaker.Core.Services.Interfaces;
using NoteTaker.DAL;

namespace NoteTaker.Core.Services.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly NoteTakerContext context;
        private readonly ILogger<UsersService> logger;
        private readonly IPasswordHashingService passwordHashingService;
        private readonly ITokenService tokenService;

        public UsersService(NoteTakerContext context, ILogger<UsersService> logger, IPasswordHashingService passwordHashingService, ITokenService tokenService)
        {
            this.context = context;
            this.logger = logger;
            this.passwordHashingService = passwordHashingService;
            this.tokenService = tokenService;
        }

        public async Task<AuthenticatedUser> AuthenticateAsync(string username, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
            {
                logger.LogDebug($"Unable to find user {username}");
                return null;
            }

            var passwordMatches = passwordHashingService.VerifyPassword(password, user.PasswordHash);

            if (!passwordMatches)
            {
                logger.LogDebug($"Password for {username} does not match");
                return null;
            }

            var token = tokenService.GetToken(user.Id.ToString(), user.Username);

            return new AuthenticatedUser(user, token);
        }

        public async Task<User> CreateUserAsync(CreateUser createUser)
        {
            if (context.Users.Any(x => x.Username == createUser.Username))
            {
                return null;
            }

            var passwordHash = passwordHashingService.GetPasswordHash(createUser.Password);

            var user = new DAL.Entities.User(createUser.Username, createUser.FirstName, createUser.LastName, passwordHash);

            context.Users.Add(user);

            await context.SaveChangesAsync();

            return new User(user);
        }
    }
}
