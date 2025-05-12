using Microsoft.AspNetCore.Mvc;
using CardGamesSolution.Server.Database;

namespace CardGamesSolution.Server.UserAccount
{
    [ApiController]
    [Route("api/usermanager")]
    public class LoginManager : ControllerBase, ILoginManager
    {
        private readonly LoginEngine _loginEngine;

        public LoginManager(LoginEngine loginEngine)
        {
            _loginEngine = loginEngine;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDto user)
        {
            try
            {
                User loggedInUser = _loginEngine.Login(user.Username, user.Password);
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
                _loginEngine.Register(user.Username, user.Password);
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
