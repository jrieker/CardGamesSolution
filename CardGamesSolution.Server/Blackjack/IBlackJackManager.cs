using Microsoft.AspNetCore.Mvc;
using CardGamesSolution.Server.UserAccount;
using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Blackjack
{
    public interface IBlackJackManager
    {
        IActionResult Start(User[] users);
        IActionResult EndRound();
        IActionResult Deal();
        IActionResult Hit(int userId);
        IActionResult Stand(int userId);
        IActionResult Double(int userId);
        IActionResult Bet(BetRequestDto data);
        IActionResult DealerStep();
    }
}
