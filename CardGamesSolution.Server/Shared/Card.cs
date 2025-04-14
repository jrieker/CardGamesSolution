namespace CardGamesSolution.Server.Shared
{
    /// <summary>
    /// Represents a single card with a suit and a number.
    /// </summary>
    public class Card {

        /// <summary>
        /// Gets the suit of the card (Hearts, Diamonds, Clubs, Spades).
        /// </summary>
        public string Suit { get; }

        /// <summary>
        /// Gets the number of the card.
        /// 1 = Ace, 11 = Jack, 12 = Queen, 13 = King. 
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// Initializes a new <see cref="Card"/> class with the given suit and number.
        /// </summary>
        /// <param name="suit">The suit of the card.</param>
        /// <param name="number">The number of the card (1 to 13).</param>
        public Card(string suit, int number)
        {
            Suit = suit;
            Number = number;
        }

        /// <summary>
        /// Returns true if the card is a red suit (Hearts or Diamonds).
        /// </summary>
        public bool IsRed() {
            return Suit == "Hearts" || Suit == "Diamonds";
        }

        /// <summary>
        /// Returns the rank symbol of the card ("A", "J", "Q", "K", or number).
        /// </summary>
        public string GetRank() {
            return Number switch {
                1 => "A",
                11 => "J",
                12 => "Q",
                13 => "K",
                _ => Number.ToString()
            };
        }
    }
}
