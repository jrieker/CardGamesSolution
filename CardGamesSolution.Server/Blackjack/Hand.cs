using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Blackjack
{
    public class Hand
    {
        private List<Card> Cards = new List<Card>();

        public Hand()
        {
        }

        public Hand(Card CardOne, Card CardTwo)
        {
            Cards.Add(CardOne);
            Cards.Add(CardTwo);
        }

        public Hand(Deck deck)
        {
            Cards.Add(deck.Draw());
            Cards.Add(deck.Draw());
        }

        public List<Card> getCards()
        {
            return Cards;
        }

        public void SetCards(List<Card> newCards)
        {
            Cards = newCards;
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public void printHand()
        {
            Console.WriteLine("Hand:");
            foreach (Card card in Cards)
            {
                Console.WriteLine($"{card.Number} of {card.Suit}");
            }
        }

        public int valueOfHand()
        {
            int total = 0;
            int numAces = 0;

            foreach (Card card in Cards)
            {
                int value = card.Number;

                if (value == 1)
                {
                    total += 11;
                    numAces += 1;
                }
                else if (value >= 11 && value <= 13)
                {
                    total += 10;
                }
                else
                {
                    total += value;
                }
            }

            while (total > 21 && numAces > 0)
            {
                total -= 10;
                numAces--;
            }

            return total;
        }
    }
}
