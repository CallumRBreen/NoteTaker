using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NoteTaker.API.Controllers;
using NoteTaker.API.ViewModels;
using NoteTaker.Core.Services.Interfaces;
using Xunit;

namespace NoteTaker.API.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly UsersController controller;
        private readonly Mock<IUsersService> userService;

        public UsersControllerTests()
        {
            userService = new Mock<IUsersService>();
            controller = new UsersController(userService.Object, new NullLogger<UsersController>());
        }

        [Fact]
        public async Task Authenticate_User_Successfully()
        {
            var userLogin = new UserLogin()
            {
                Username = "JohnSmith2019",
                Password = "apples"
            };

            userService.Setup(x => x.AuthenticateAsync(It.Is<string>(y => y.Equals(userLogin.Username)),It.Is<string>(y => y.Equals(userLogin.Password)))).ReturnsAsync(new Core.Models.AuthenticatedUser
            {
                Id = Guid.NewGuid().ToString(),
                Username = userLogin.Username,
                FirstName = "John",
                LastName = "Smith",
                Modified = DateTime.Now,
                Created = DateTime.Now,
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"
            }).Verifiable();

            var result = (await controller.Login(userLogin)).Result as OkObjectResult;

            var authenticatedUser = (AuthenticatedUser) result?.Value;

            Assert.NotNull(authenticatedUser);
            userService.Verify();
        }

        [Fact]
        public async Task Failed_Authentication_Returns_Bad_Request()
        {
            var userLogin = new UserLogin()
            {
                Username = "JohnSmith2019",
                Password = "apples"
            };

            userService.Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null).Verifiable();

            var result = (await controller.Login(userLogin)).Result as BadRequestObjectResult;

            var errorMessage = result?.Value;

            Assert.Equal("Username or password is incorrect", errorMessage);
        }
    }
}
