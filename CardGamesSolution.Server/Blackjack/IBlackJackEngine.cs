using CardGamesSolution.Server.Shared;
using CardGamesSolution.Server.UserAccount;

namespace CardGamesSolution.Server.Blackjack
{
    public interface IBlackJackEngine
    {
        List<Player> Intialize(User[] users);

        int GetCurrentTurnIndex();

        object DealInitialCards();

        object RegisterBet(string username, float amount);

        object Hit(int userId);

        object Stand(int userId);

        object Double(int userId);

        object DealerStep();

        object EndRound(IUserDataAccessor userDataAccessor);

        float ComputePayout(int playerValue, int dealerValue, float bet);
    }
}
