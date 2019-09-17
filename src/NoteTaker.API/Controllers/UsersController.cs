using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NoteTaker.API.ViewModels;
using NoteTaker.Core.Services.Interfaces;

namespace NoteTaker.API.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly ILogger<UsersController> logger;

        public UsersController(IUsersService usersService, ILogger<UsersController> logger)
        {
            this.usersService = usersService;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticatedUser>> Login([FromBody] UserLogin userLogin)
        {
            logger.LogDebug($"Attempting to authenticate user {userLogin.Username}");

            var authenticatedUser = await usersService.AuthenticateAsync(userLogin.Username, userLogin.Password);

            if (authenticatedUser == null)
            {
                logger.LogDebug($"Failed to authenticate user {userLogin.Username}");
                return BadRequest("Username or password is incorrect");
            }

            return Ok(new AuthenticatedUser(authenticatedUser));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] CreateUser createUser)
        {
            logger.LogDebug($"Attempting to create user with username {createUser.Username}");

            var createdUser = await usersService.CreateUserAsync(createUser.ToCreateUser());

            if (createdUser == null)
            {
                logger.LogDebug($"Unable to create user {createUser.Username}");
                return BadRequest($"Username {createUser.Username} already taken");
            }

            return Created($"api/users/{createdUser.Id}", new User(createdUser));
        }
    }
}
