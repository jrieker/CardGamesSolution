public class Card
{
    // gets the suit of the card
    public string Suit { get; }

    // gets the number of the card
    public int Number { get; }

    // initializes a new Card with the given suit and number
    
    public Card(string suit, int number)
    {
        Suit = suit;
        Number = number;

        // creates card object with suit and number
    }

    public override string ToString()
    {
        return $"{GetRank()} of {Suit}";

        // formatting card
    }

    private string GetRank()
    {
        return Number switch
        {
            1 => "A",
            11 => "J",
            12 => "Q",
            13 => "K",
            _ => Number.ToString()
        };
        
        // converts card number to rank
    }
    public bool IsRed() {
    return Suit == "Hearts" || Suit == "Diamonds";

    // returns true if card is red
}
}