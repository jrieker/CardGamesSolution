using System.Numerics;
using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Blackjack
{
    internal interface IBlackJackAccessor : ICardGamesAccessor
    {
        public Player LoadUserInfo(int PlayerId);

        public void UpdateUserInfo(Player player, int wins, int losses, float balance);

        public void updateUserEarning(int playerId);

    }
}
