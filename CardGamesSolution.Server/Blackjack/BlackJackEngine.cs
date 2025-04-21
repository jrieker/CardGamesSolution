using CardGamesSolution.Server.Shared;
using System.Numerics;

namespace CardGamesSolution.Server.Blackjack
{
    public class BlackJackEngine : IBlackJackEngine
    {
        public void DealHands(Player[] players, Hand dealerHand, Deck deck)
        {
            foreach (var player in players)
            {
                Card cardOne = deck.Draw();
                Card cardTwo = deck.Draw();
                Hand newHand = new Hand(cardOne, cardTwo);
                player.SetPlayerHand(newHand);
            }
           
            dealerHand.SetCards();
            dealerHand.AddCard(deck.Draw());
            dealerHand.AddCard(deck.Draw());
        }

        public int Hit(Hand hand, Deck deck)
        {
            var card = deck.Draw();
            hand.AddCard(card);
            return hand.valueOfHand();

        }

        public int PlayDealer(Hand dealerHand, Deck deck)
        {
            while (dealerHand.valueOfHand() < 17)
            {
                dealerHand.AddCard(deck.Draw());
            }
            return dealerHand.valueOfHand();
        }

        public float ComputePayout(int playerValue, int dealerValue, float bet)
        {
            if (playerValue > 21)
            {
                return -bet;
            }
            if (dealerValue > 21)
            {
                return +bet;
            }
            if (playerValue > dealerValue)
            {
                return +bet;
            }
            if (playerValue < dealerValue)
            {
                return -bet; ;
            }
            return 0;
        }




    }
}
