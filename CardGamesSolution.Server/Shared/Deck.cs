namespace CardGamesSolution.Server.Shared
{

    /// <summary>
    /// Represents a deck of 52 cards.
    /// </summary>
    public class Deck
    {

        private List<Card> cards;
        private static readonly string[] Suits = { "Hearts", "Diamonds", "Clubs", "Spades" };

        /// <summary>
        /// Initializes a new deck with 52 cards (13 in each suit).
        /// </summary>
        public Deck()
        {
            cards = new List<Card>();
            foreach (var suit in Suits)
            {
                for (int n = 1; n <= 13; n++)
                {
                    cards.Add(new Card(suit, n));
                }
            }
        }

        /// <summary>
        /// Shuffles the deck.
        /// </summary>
        public void Shuffle()
        {
            Random ran = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = ran.Next(n + 1);
                (cards[n], cards[k]) = (cards[k], cards[n]);
            }
        }

        /// <summary>
        /// Removes and returns the top card of the deck.
        /// Returns null if the deck is empty.
        /// </summary>
        /// <returns>The top <see cref="Card"/> or null if the deck is empty.</returns>
        public Card Draw()
        {
            if (cards.Count == 0)
            {
                Console.WriteLine("No cards left. Create a new deck"); // Temporarily writing this to console
                return null;
            }

            Card topCard = cards[0];
            cards.RemoveAt(0);
            return topCard;
        }

        /// <summary>
        /// Rebuilds the deck to contain all 52 cards (unshuffled).
        /// </summary>
        private void ResetDeck()
        {
            cards.Clear();
            foreach (var suit in Suits)
            {
                for (int n = 1; n <= 13; n++)
                {
                    cards.Add(new Card(suit, n));
                }
            }
        }
    }
}
