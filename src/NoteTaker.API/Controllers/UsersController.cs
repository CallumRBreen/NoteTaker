using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NoteTaker.API.ViewModels;
using NoteTaker.Core.Models;
using NoteTaker.Core.Services.Interfaces;

namespace NoteTaker.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ILogger<UsersController> logger;

        public UsersController(IUsersService usersService, ILogger<UsersController> logger)
        {
            this._usersService = usersService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<AuthenticatedUser>> Login([FromBody] UserLogin userLogin)
        {
            logger.LogDebug($"Attempting to authenticate user {userLogin.Username}");

            var authenticatedUser = await _usersService.AuthenticateAsync(userLogin.Username, userLogin.Password);

            if (authenticatedUser == null)
            {
                logger.LogDebug($"Failed to authenticate user {userLogin.Username}");
                return BadRequest("Username or password is incorrect");
            }

            return Ok(authenticatedUser);
        }
    }
}
