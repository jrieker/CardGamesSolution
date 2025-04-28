using CardGamesSolution.Server.Solitaire;
using CardGamesSolution.Server.UserAccount;
using Microsoft.AspNetCore.Mvc;

namespace CardGamesSolution.Server.Blackjack
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlackJackController : ControllerBase
    {
        private readonly IBlackJackManager manager;

        public BlackJackController(IBlackJackManager newManager)
        {
            manager = newManager;
        }

       
        [HttpPost("start")]
        public ActionResult<MultiplayerGameState> Start([FromBody] User[] users)
        {
            if (users == null || users.Length == 0)
                return BadRequest("At least one user is required to start a game.");

            MultiplayerGameState state = manager.InitializeGame(users);
            return Ok(state);
        }

        [HttpPost("bet")]
        public ActionResult<ActionResultDto> Bet([FromBody] BetCommandDto cmd)
        {
            ActionResultDto result = manager.PlaceBet(cmd);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost("hit")]
        public ActionResult<ActionResultDto> Hit([FromBody] HitCommandDto cmd)
        {
            ActionResultDto result = manager.Hit(cmd);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("stand")]
        public ActionResult<ActionResultDto> Stand([FromBody] StandCommandDto cmd)
        {
            ActionResultDto result = manager.Stand(cmd);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("state")]
        public ActionResult<MultiplayerGameState> State()
        {
            MultiplayerGameState state = manager.GetState();
            return Ok(state);
        }
    
    }
}
