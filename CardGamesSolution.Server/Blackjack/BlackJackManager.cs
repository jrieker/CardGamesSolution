using CardGamesSolution.Server.Shared;
using CardGamesSolution.Server.UserAccount;
using Microsoft.AspNetCore.Mvc;

namespace CardGamesSolution.Server.Blackjack
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlackJackManager : ControllerBase
    {
        private readonly BlackJackEngine _engine;

        public BlackJackManager(BlackJackEngine engine)
        {
            _engine = engine;
        }

        [HttpPost("start")]
        public IActionResult Start([FromBody] User[] users)
        {
            if (users == null || users.Length == 0)
                return BadRequest("At least one user is required to start a game.");

            var initializedPlayers = _engine.Intialize(users);

            var result = initializedPlayers.Select(p => new PlayerDto
            {
                UserId = p.PlayerId,
                Username = p.PlayerName,
                Balance = p.Balance
            }).ToList();

            return Ok(new
            {
                players = result,
                currentTurnIndex = _engine.GetCurrentTurnIndex()
            });
        }

        [HttpPost("deal")]
        public IActionResult Deal() => Ok(_engine.DealInitialCards());

        [HttpPost("hit")]
        public IActionResult Hit([FromBody] int userId) => Ok(_engine.Hit(userId));

        [HttpPost("stand")]
        public IActionResult Stand([FromBody] int userId) => Ok(_engine.Stand(userId));

        [HttpPost("double")]
        public IActionResult Double([FromBody] int userId) => Ok(_engine.Double(userId));

        [HttpPost("bet")]
        public IActionResult Bet([FromBody] BetRequestDto data) =>
            Ok(_engine.RegisterBet(data.Username, data.Amount));

        [HttpPost("dealer/step")]
        public IActionResult DealerStep() => Ok(_engine.DealerStep());

        [HttpPost("end")]
        public IActionResult EndRound() => Ok(_engine.EndRound());
    }
}
