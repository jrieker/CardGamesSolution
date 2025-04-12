using System.Collections.Generic;
using CardGamesSolution.Server.Shared;


namespace CardGamesSolution.Server.Solitaire {
    public class GameState {
        public List<TableauPile> Tableau { get; set; }
        public Dictionary<string, List<Card>> Foundation { get; set; }
        public List<Card> Stock { get; set; }
        public List<Card> Waste { get; set; }
        public int Score { get; set; }

        // holds all live game data

        public GameState() {
            Tableau = new List<TableauPile>();
            for (int i = 0; i < 7; i++) {
                Tableau.Add(new TableauPile());

                // initializes 7 tableau columns each as empty Pile
            }

            Foundation = new Dictionary<string, List<Card>> {
                { "Hearts", new List<Card>() },
                { "Diamonds", new List<Card>() },
                { "Clubs", new List<Card>() },
                { "Spades", new List<Card>() }

                // sets up four foundation piles, 1 per suit
            };

            Stock = new List<Card>();
            Waste = new List<Card>();
            Score = 0;

            // Initializes stock waste and score if we want to add score
        }
    }
}
