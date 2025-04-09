using System;
using System.Collections.Generic;

// represents a standard deck of 52 cards

public class Deck
{
    private List<Card> cards;
    private static readonly string[] Suits = { "Hearts", "Diamonds", "Clubs", "Spades" };

    public Deck()
    {
        cards = new List<Card>();
        foreach (var suit in Suits)
        {
            for (int num = 1; num <= 13; num++)
            {
                cards.Add(new Card(suit, num));
            }
        }

        // initializes empty list of cards, loops thru each suit, for each suit creates 13 cards, adds each card to list
    }

    public void Shuffle()
    {
        Random rand = new Random();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }

        // shuffles cards
    }

    public Card? Draw()
    {
        if (cards.Count == 0)
            return null;

        Card topCard = cards[0];
        cards.RemoveAt(0);
        return topCard;

        // returns top card from deck, if deck is empty it returns null
    }

    public List<Card> GetCards()
    {
        return new List<Card>(cards);

        // returns copy of current deck as a list
    }

    public void Reset()
    {
        cards.Clear();
        foreach (var suit in Suits)
        {
            for (int num = 1; num <= 13; num++)
            {
                cards.Add(new Card(suit, num));

                // clears existing deck, gives you brand new deck of 52 cards
            }
        }
    }
}
