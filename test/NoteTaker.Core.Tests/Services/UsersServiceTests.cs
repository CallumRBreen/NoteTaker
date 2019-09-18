using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NoteTaker.Core.Models;
using NoteTaker.Core.Services.Implementations;
using NoteTaker.Core.Services.Interfaces;
using NoteTaker.Core.Tests.Helpers;
using NoteTaker.DAL;
using Xunit;
using User = NoteTaker.DAL.Entities.User;

namespace NoteTaker.Core.Tests.Services
{
    public class UsersServiceTests
    {
        private readonly Mock<IPasswordHashingService> passwordHashingService;
        private readonly Mock<ITokenService> tokenService;
        private readonly NullLogger<UsersService> logger;
        private readonly Mock<IHttpContextAccessor> httpContextAccessor;

        public UsersServiceTests()
        {
            passwordHashingService = new Mock<IPasswordHashingService>();
            tokenService = new Mock<ITokenService>();
            logger = new NullLogger<UsersService>();
            httpContextAccessor = new Mock<IHttpContextAccessor>();
        }

        [Fact]
        public async Task Return_Token_After_Authenticated_User_Successfully()
        {
            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Return_Token_After_Authenticated_User_Successfully));

            var password = "Apples";
            var passwordHash = Guid.NewGuid().ToString();

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                context.Users.Add(new User("JohnSmith", "John", "Smith", passwordHash));
                context.SaveChanges();
            }

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                passwordHashingService.Setup(x => x.VerifyPassword(It.Is<string>(y => y.Equals(password)),
                            It.Is<string>(y => y.Equals(passwordHash)))).Returns(true).Verifiable();

                tokenService.Setup(x => x.GetToken(It.IsAny<string>(), It.IsAny<string>())).Returns(Guid.NewGuid().ToString).Verifiable();

                var usersService = new UsersService(context, logger, passwordHashingService.Object, tokenService.Object);

                var authenticatedUser = await usersService.AuthenticateAsync("JohnSmith", password);

                authenticatedUser.Should().NotBeNull();
                authenticatedUser.Token.Should().NotBeNull();

                passwordHashingService.Verify();
                tokenService.Verify();
            }
        }

        [Fact]
        public async Task Return_Null_After_Failed_Authentication()
        {
            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Return_Null_After_Failed_Authentication));

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                context.Users.Add(new User("JohnSmith", "John", "Smith", Guid.NewGuid().ToString()));
                context.SaveChanges();
            }

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                passwordHashingService.Setup(x => x.VerifyPassword(It.IsAny<string>(),It.IsAny<string>())).Returns(false).Verifiable();

                var usersService = new UsersService(context, logger, passwordHashingService.Object, tokenService.Object);

                var authenticatedUser = await usersService.AuthenticateAsync("JohnSmith", "Apples");

                authenticatedUser.Should().BeNull();

                passwordHashingService.Verify();
            }
        }

        [Fact]
        public async Task Create_User_Successfully()
        {
            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Create_User_Successfully));

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                var createUser = new CreateUser()
                {
                    Username = "JohnSmith",
                    FirstName = "John",
                    LastName = "Smith",
                    Password = "Apples"
                };

                passwordHashingService.Setup(x => x.GetPasswordHash(It.Is<string>(y => y.Equals("Apples")))).Returns(Guid.NewGuid().ToString).Verifiable();

                var usersService = new UsersService(context, logger, passwordHashingService.Object, tokenService.Object);

                var createdUser = await usersService.CreateUserAsync(createUser);

                createdUser.Id.Should().NotBeNull();

                passwordHashingService.Verify();
            }
        }

        [Fact]
        public async Task Ensure_Unique_Username()
        {
            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Ensure_Unique_Username));

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "JohnSmith",
                    FirstName = "John",
                    LastName = "Smith",
                    InternalId = 1,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    PasswordHash = Guid.NewGuid().ToString()
                };

                context.Users.Add(user);
                context.SaveChanges();
            }

            using (var context = new NoteTakerContext(options, httpContextAccessor.Object))
            {
                var createUser = new CreateUser()
                {
                    Username = "JohnSmith",
                    FirstName = "John",
                    LastName = "Smith",
                    Password = "Apples"
                };

                var usersService = new UsersService(context, logger, passwordHashingService.Object, tokenService.Object);

                var createdUser = await usersService.CreateUserAsync(createUser);

                createdUser.Should().BeNull();
            }
        }

    }
}
