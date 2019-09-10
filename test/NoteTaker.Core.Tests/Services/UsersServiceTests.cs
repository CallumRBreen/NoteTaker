using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NoteTaker.Core.Services.Implementations;
using NoteTaker.Core.Services.Interfaces;
using NoteTaker.Core.Tests.Helpers;
using NoteTaker.DAL;
using NoteTaker.DAL.Entities;
using Xunit;

namespace NoteTaker.Core.Tests.Services
{
    public class UsersServiceTests
    {
        private readonly Mock<IPasswordHashingService> passwordHashingService;
        private readonly Mock<ITokenService> tokenService;
        private readonly NullLogger<UsersService> logger;

        public UsersServiceTests()
        {
            passwordHashingService = new Mock<IPasswordHashingService>();
            tokenService = new Mock<ITokenService>();
            logger = new NullLogger<UsersService>();
        }

        [Fact]
        public async Task Return_Token_After_Authenticated_User_Successfully()
        {
            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Return_Token_After_Authenticated_User_Successfully));

            var password = "Apples";
            var passwordHash = Guid.NewGuid().ToString();

            using (var context = new NoteTakerContext(options))
            {
                context.Users.Add(new User("JohnSmith", "John", "Smith", passwordHash));
                context.SaveChanges();
            }

            using (var context = new NoteTakerContext(options))
            {
                passwordHashingService.Setup(x => x.VerifyPassword(It.Is<string>(y => y.Equals(password)),
                            It.Is<string>(y => y.Equals(passwordHash)))).Returns(true).Verifiable();

                tokenService.Setup(x => x.GetToken(It.IsAny<string>())).Returns(Guid.NewGuid().ToString).Verifiable();

                var usersService = new UsersService(context, logger, passwordHashingService.Object, tokenService.Object);

                var authenticatedUser = await usersService.AuthenticateAsync("JohnSmith", password);

                Assert.NotNull(authenticatedUser);
                Assert.NotNull(authenticatedUser.Token);

                passwordHashingService.Verify();
                tokenService.Verify();
            }
        }

        [Fact]
        public async Task Return_Null_After_Failed_Authentication()
        {
            var options = DbContextHelper.GetTestInMemoryDatabase(nameof(Return_Null_After_Failed_Authentication));

            using (var context = new NoteTakerContext(options))
            {
                context.Users.Add(new User("JohnSmith", "John", "Smith", Guid.NewGuid().ToString()));
                context.SaveChanges();
            }

            using (var context = new NoteTakerContext(options))
            {
                passwordHashingService.Setup(x => x.VerifyPassword(It.IsAny<string>(),It.IsAny<string>())).Returns(false).Verifiable();

                var usersService = new UsersService(context, logger, passwordHashingService.Object, tokenService.Object);

                var authenticatedUser = await usersService.AuthenticateAsync("JohnSmith", "Apples");

                Assert.Null(authenticatedUser);

                passwordHashingService.Verify();
            }
        }
    }
}
