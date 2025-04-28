using System.Numerics;
using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Blackjack
{
    public interface IBlackJackEngine
    {
        public void DealHands(List<Player> players, Hand dealerHand, Deck deck);

        public int Hit(Hand hand, Deck deck);


        public int PlayDealer(Hand dealerHand, Deck deck);


        public float ComputePayout(int playerValue, int dealerValue, float bet);

    }
}
