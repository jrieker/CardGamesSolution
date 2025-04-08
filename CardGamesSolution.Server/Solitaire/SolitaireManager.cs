using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Solitaire
{
    public class SolitaireManager : ISolitaireManager
    {
        private ISolitaireEngine engine;
        private List<Card> deck;
        // filled deck

        public SolitaireManager(ISolitaireEngine solitaireEngine)
        {
            engine = solitaireEngine;
            deck = GenerateDeck();

            // constructor, new deck of cards
        }

        public void StartNewGame()
        {
            Shuffle(deck);
            engine.InitializeGame(new List<Card>(deck));

            // starts new game shuffles deck and deals cards
        }

        public void DealCards()
        {
            // we can use this if needed to be seperated when using UI, for animations 
        }

        public void Move(Card card, string fromPile, string toPile)
        {
            var result = engine.ProcessMove(card, fromPile, toPile);
            Console.WriteLine(result.Message);

            // checks if the move a player trys to do is valid, prints to console
        }

        public void Restart()
        {
            StartNewGame();
        }

        public GameState GetGameState()
        {
            return engine.GetGameState();

            // gets current state, tableau, foundation, stock etc.
        }

        private List<Card> GenerateDeck()
        {
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

            List<Card> fullDeck = new();

            foreach (var suit in suits)
                foreach (var rank in ranks)
                    fullDeck.Add(new Card(suit, rank)); // adds each deck combo to deck

            return fullDeck;

            // creates and returns a 52 card deck, 
        }

        private void Shuffle(List<Card> cards)
        {
            Random rand = new Random();
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]);

                // randomly shuffles the deck of cards
            }
        }
    }
}
