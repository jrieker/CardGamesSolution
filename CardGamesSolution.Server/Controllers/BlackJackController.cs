using CardGamesSolution.Server.UserAccount;
using CardGamesSolution.Server.Blackjack;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CardGamesSolution.Server.Database;

namespace CardGamesSolution.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlackJackController : ControllerBase
    {
        private readonly BlackJackManager manager;
        private readonly IUserDataAccessor userDataAccessor;
        public BlackJackController(BlackJackManager newManager, IUserDataAccessor newUserDataAccessor)
        {
            manager = newManager;
            userDataAccessor = newUserDataAccessor;
        }

        [HttpPost("start")]
        public IActionResult Start([FromBody] User[] users)
        {
            Console.WriteLine("Starting game");
            if (users == null || users.Length == 0)
                return BadRequest("At least one user is required to start a game.");

            var initializedPlayers = manager.Intialize(users);

            var result = initializedPlayers.Select(p => new PlayerDto
            {
                UserId = p.PlayerId,
                Username = p.PlayerName,
                Balance = p.Balance 
            }).ToList();

            return Ok(new
            {
                players = result,
                currentTurnIndex = manager.GetCurrentTurnIndex()
            });
        }

        [HttpPost("end")]
        public IActionResult EndRound()
        {
            var result = manager.EndRound(userDataAccessor);
            return Ok(result);
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
