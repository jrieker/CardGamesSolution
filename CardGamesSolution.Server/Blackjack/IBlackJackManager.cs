using CardGamesSolution.Server.Shared;
using CardGamesSolution.Server.UserAccount;
using Microsoft.AspNetCore.Mvc;

namespace CardGamesSolution.Server.Blackjack
{
    public interface IBlackJackManager
    {
        IActionResult Start([FromBody] User[] users);
        IActionResult Deal();
        IActionResult Hit([FromBody] int userId);
        IActionResult Stand([FromBody] int userId);
        IActionResult Double([FromBody] int userId);
        IActionResult Bet([FromBody] BetRequestDto data);
        IActionResult DealerStep();
        IActionResult EndRound();
    }
}
