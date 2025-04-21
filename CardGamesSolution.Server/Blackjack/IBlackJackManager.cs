using System.Numerics;
using CardGamesSolution.Server.Shared;
using CardGamesSolution.Server.UserAccount;

namespace CardGamesSolution.Server.Blackjack
{
    public interface IBlackJackManager : ICardGamesManager
    {
        MultiplayerGameState InitializeGame(User[] users);
        ActionResultDto PlaceBet(BetCommandDto cmd);
        ActionResultDto Hit(HitCommandDto cmd);
        ActionResultDto Stand(StandCommandDto cmd);
        MultiplayerGameState GetState();
    }
}
