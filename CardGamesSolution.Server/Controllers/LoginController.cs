using Microsoft.AspNetCore.Mvc;
using CardGamesSolution.Server.UserAccount;

namespace CardGamesSolution.Server.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginManager _loginManager;

        public LoginController(ILoginManager loginManager)
        {
            _loginManager = loginManager;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDto user)
        {
            try
            {
                var result = _loginManager.Login(user.Username, user.Password);
                return Ok(new
                {
                    success = true,
                    userId = result.UserId,
                    username = result.Username,
                    password = result.Password,
                    balance = result.Balance,
                    wins = result.Wins,
                    losses = result.Losses
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
}
