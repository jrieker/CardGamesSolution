using Microsoft.AspNetCore.Mvc;
using CardGamesSolution.Server.Shared;
using CardGamesSolution.Server.Solitaire;

namespace CardGamesSolution.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolitaireController : ControllerBase
    {
        private static readonly SolitaireService _service = new SolitaireService();

        [HttpPost("start")]
        public IActionResult StartGame()
        {
            try
            {
                _service.StartGame();
                return Ok(new { success = true, message = "Game started." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("state")]
        public IActionResult GetGameState()
        {
            try
            {
                var state = _service.GetState();
                return Ok(new { success = true, state });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("draw")]
        public IActionResult DrawFromStock()
        {
            try
            {
                _service.DrawFromStock();
                return Ok(new { success = true, message = "Card drawn from stock." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("move")]
        public IActionResult MoveCard([FromBody] MoveDto move)
        {
            try
            {
                MoveResult result = _service.MoveCard(move.Card, move.FromPile, move.ToPile);
                return Ok(new
                {
                    success = result.IsValid,
                    message = result.Message,
                    gameState = result.GameState
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }

    public class MoveDto
    {
        public Card Card { get; set; }
        public string FromPile { get; set; }
        public string ToPile { get; set; }
    }
}
