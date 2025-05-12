using CardGamesSolution.Server.UserAccount;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CardGamesSolution.Server.Blackjack
{
    [ApiController]
    [Route("api/blackjack")]
    public class BlackJackManager : ControllerBase, IBlackJackManager
    {
        private readonly BlackJackEngine engine;
        private readonly IUserDataAccessor userDataAccessor;

        public BlackJackManager(BlackJackEngine newEngine, IUserDataAccessor newUserDataAccessor)
        {
            engine = newEngine;
            userDataAccessor = newUserDataAccessor;
        }

        [HttpPost("start")]
        public IActionResult Start([FromBody] User[] users)
        {
            Console.WriteLine("Starting game");
            if (users == null || users.Length == 0)
                return BadRequest("At least one user is required to start a game.");

            var initializedPlayers = engine.Intialize(users);

            var result = initializedPlayers.Select(p => new PlayerDto
            {
                UserId = p.PlayerId,
                Username = p.PlayerName,
                Balance = p.Balance
            }).ToList();

            return Ok(new
            {
                players = result,
                currentTurnIndex = engine.GetCurrentTurnIndex()
            });
        }

        [HttpPost("end")]
        public IActionResult EndRound()
        {
            var result = engine.EndRound(userDataAccessor);
            return Ok(result);
        }

        [HttpPost("deal")]
        public IActionResult Deal()
        {
            var result = engine.DealInitialCards();
            return Ok(result);
        }

        [HttpPost("hit")]
        public IActionResult Hit([FromBody] int userId)
        {
            var result = engine.Hit(userId);
            return Ok(result);
        }

        [HttpPost("stand")]
        public IActionResult Stand([FromBody] int userId)
        {
            var result = engine.Stand(userId);
            return Ok(result);
        }

        [HttpPost("double")]
        public IActionResult Double([FromBody] int userId)
        {
            var result = engine.Double(userId);
            return Ok(result);
        }

        [HttpPost("bet")]
        public IActionResult Bet([FromBody] BetRequestDto data)
        {
            var result = engine.RegisterBet(data.Username, data.Amount);
            return Ok(result);
        }

        [HttpPost("dealer/step")]
        public IActionResult DealerStep()
        {
            var result = engine.DealerStep();
            return Ok(result);
        }
    }
}
