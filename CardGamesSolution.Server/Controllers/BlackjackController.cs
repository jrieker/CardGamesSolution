using CardGamesSolution.Server.UserAccount;
using CardGamesSolution.Server.Blackjack;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CardGamesSolution.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlackJackController : ControllerBase
    {
        private readonly BlackJackManager manager;

        public BlackJackController(BlackJackManager newManager)
        {
            manager = newManager;
        }

        [HttpPost("start")]
        public IActionResult Start([FromBody] User[] users)
        {
            if (users == null || users.Length == 0)
                return BadRequest("At least one user is required to start a game.");

            var initializedPlayers = manager.Intialize(users);

            var result = initializedPlayers.Select(p => new PlayerDto
            {
                UserId = p.PlayerId,
                Username = p.PlayerName
            }).ToList();

            return Ok(new
            {
                players = result,
                currentTurnIndex = manager.GetCurrentTurnIndex()
            });
        }

        [HttpPost("deal")]
        public IActionResult Deal()
        {
            var result = manager.DealInitialCards();
            return Ok(result);
        }

        [HttpPost("hit")]
        public IActionResult Hit([FromBody] int userId)
        {
            var result = manager.Hit(userId);
            return Ok(result);
        }

        [HttpPost("stand")]
        public IActionResult Stand([FromBody] int userId)
        {
            var result = manager.Stand(userId);
            return Ok(result);
        }

        [HttpPost("double")]
        public IActionResult Double([FromBody] int userId)
        {
            var result = manager.Double(userId);
            return Ok(result);
        }

        [HttpPost("bet")]
        public IActionResult Bet([FromBody] BetRequestDto data)
        {
            var result = manager.RegisterBet(data.Username, data.Amount);
            return Ok(result);
        }

        [HttpPost("dealer/step")]
        public IActionResult DealerStep()
        {
            var result = manager.DealerStep();
            return Ok(result);
        }
    }
}
