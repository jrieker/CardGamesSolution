using Microsoft.AspNetCore.Mvc;
using CardGamesSolution.Server.UserAccount;
using CardGamesSolution.Server.Database;

namespace CardGamesSolution.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly LoginManager _loginManager;

        public UserController(LoginManager loginManager)
        {
            _loginManager = loginManager;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDto user)
        {
            try
            {
                User loggedInUser = _loginManager.Login(user.Username, user.Password);
                return Ok(new
                {
                    success = true,
                    userId = loggedInUser.UserId,
                    username = loggedInUser.Username,
                    password = loggedInUser.Password,
                    balance = loggedInUser.Balance,
                    wins = loggedInUser.Wins,
                    losses = loggedInUser.Losses
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDto user)
        {
            try
            {
                _loginManager.Register(user.Username, user.Password);
                return Ok(new { success = true, username = user.Username });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }

    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
