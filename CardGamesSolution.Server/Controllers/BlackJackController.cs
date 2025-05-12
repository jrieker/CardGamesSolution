using CardGamesSolution.Server.Blackjack;
using CardGamesSolution.Server.UserAccount;
using Microsoft.AspNetCore.Mvc;

namespace CardGamesSolution.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlackJackController : ControllerBase
    {
        private readonly IBlackJackManager manager;

        public BlackJackController(IBlackJackManager blackjackManager)
        {
            manager = blackjackManager;
        }

        [HttpPost("start")]
        public IActionResult Start([FromBody] User[] users)
        {
            var result = manager.Start(users);
            return Ok(result);
        }

        [HttpPost("end")]
        public IActionResult EndRound()
        {
            var result = manager.EndRound();
            return Ok(result);
        }

        [HttpPost("deal")]
        public IActionResult Deal()
        {
            var result = manager.Deal();
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
            var result = manager.Bet(data.Username, data.Amount);
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
