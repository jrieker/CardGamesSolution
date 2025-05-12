using CardGamesSolution.Server.UserAccount;
using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Blackjack
{
    public interface IBlackJackManager
    {
        object Start(User[] users);
        object EndRound();
        object Deal();
        object Hit(int userId);
        object Stand(int userId);
        object Double(int userId);
        object Bet(string username, float amount);
        object DealerStep();
    }
}
