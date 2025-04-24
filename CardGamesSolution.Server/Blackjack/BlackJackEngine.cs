using CardGamesSolution.Server.Shared;
using System.Numerics;

namespace CardGamesSolution.Server.Blackjack
{
    public class BlackJackEngine : IBlackJackEngine
    {
        public void DealHands(List<Player> players, Hand dealerHand, Deck deck)
        {
            foreach (var player in players)
            {
                var card = deck.Draw();
                player.GetPlayerHand().AddCard(card);
            }

            var dealerCardOne = deck.Draw();
            dealerHand.AddCard(dealerCardOne);

            foreach (var player in players)
            {
                var card = deck.Draw();
                player.GetPlayerHand().AddCard(card);
            }

            var dealerCardTwo = deck.Draw();
            dealerHand.AddCard(dealerCardTwo);

            Console.WriteLine("\n--- Hands Dealt ---");
            foreach (var player in players)
            {
                Console.Write($"{player.GetPlayerName()}: ");
                foreach (var card in player.GetPlayerHand().getCards())
                {
                    Console.Write($"{card.Number} of {card.Suit}, ");
                }
                Console.WriteLine();
            }

            Console.Write("Dealer: ");
            var dealerCards = dealerHand.getCards();
            Console.WriteLine($"{dealerCards[0].Number} of {dealerCards[0].Suit}, [face down]");
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

        public (Card? drawnCard, int handValue, bool shouldContinue) DealerStepDraw(Hand dealerHand, Deck deck, List<Player> players)
        {
            var card = deck.Draw();
            if (card == null)
                return (null, dealerHand.valueOfHand(), false);

            dealerHand.AddCard(card);
            int dealerValue = dealerHand.valueOfHand();

            Console.WriteLine($"Dealer draws {card.Number} of {card.Suit} (Total: {dealerValue})");

            if (dealerValue >= 21)
                return (card, dealerValue, false); 

            foreach (var player in players)
            {
                int playerValue = player.GetPlayerHand().valueOfHand();
                if (playerValue <= 21 && playerValue >= dealerValue)
                {
                    return (card, dealerValue, true); 
                }
            }

            return (card, dealerValue, false);
        }

        public float ComputePayout(int playerValue, int dealerValue, float bet)
        {
            if (playerValue > 21) return -bet;
            if (dealerValue > 21) return +bet;
            if (playerValue > dealerValue) return +bet;
            if (playerValue < dealerValue) return -bet;
            return 0;
        }
    }
}
