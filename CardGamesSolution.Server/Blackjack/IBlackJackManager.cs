using System.Numerics;
using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Blackjack
{
    public interface IBlackJackManager : ICardGamesManager
    {
        void StartGame(Player[] players, Hand dealersHand, Deck deck);


        void PlaceBets(Player[] players);


        void Payout(Player[] players, Hand dealersHand);

        void Turn(Player player, Deck deck);
    }
}
