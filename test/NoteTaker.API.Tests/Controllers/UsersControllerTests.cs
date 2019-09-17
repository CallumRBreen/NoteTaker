using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NoteTaker.API.Controllers;
using NoteTaker.API.ViewModels;
using NoteTaker.Core.Models;
using NoteTaker.Core.Services.Interfaces;
using Xunit;
using AuthenticatedUser = NoteTaker.API.ViewModels.AuthenticatedUser;
using CreateUser = NoteTaker.API.ViewModels.CreateUser;
using User = NoteTaker.API.ViewModels.User;

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

        [Fact]
        public async Task Create_User_Successfully()
        {
            var createdUserId = Guid.NewGuid().ToString();

            var createUser = new CreateUser
            {
                Username = "JohnSmith",
                FirstName = "John",
                LastName = "Smith",
                Password = "Apples"
            };

            userService.Setup(x => x.CreateUserAsync(It.IsAny<Core.Models.CreateUser>())).ReturnsAsync(
            new Core.Models.User()
            {
                Id = createdUserId,
                Username ="JohnSmith",
                FirstName = "John",
                LastName = "Smith",
                Created = DateTime.Now,
                Modified = DateTime.Now
            }).Verifiable();

            var result = (await controller.Create(createUser)).Result as CreatedResult;

            var user = (User)result?.Value;

            user.Should().NotBeNull();
            user?.Id.Should().BeEquivalentTo(createdUserId);
        }

        [Fact]
        public async Task Duplicate_Username_Returns_BadRequest()
        {
            var createUser = new CreateUser
            {
                Username = "JohnSmith",
                FirstName = "John",
                LastName = "Smith",
                Password = "Apples"
            };

            userService.Setup(x => x.CreateUserAsync(It.IsAny<Core.Models.CreateUser>())).ReturnsAsync(() => null).Verifiable();

            var result = (await controller.Create(createUser)).Result as BadRequestObjectResult;

            var response = (string)result?.Value;

            response.Should().BeEquivalentTo("Username JohnSmith already taken");
        }
    }
}
