using System;
using System.Collections.Generic;
using CardGamesSolution.Server.Shared;


namespace CardGamesSolution.Server.Solitaire {
    public class SolitaireAccessor : ISolitaireAccessor {
        public List<Card> GetCards() {
            var cards = new List<Card>();
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };

            for (int i = 1; i <= 13; i++) {
                foreach (var suit in suits) {
                    cards.Add(new Card(suit, i));
                }
            }

            return cards;
        }

        public Dictionary<string, string> GetCardImages() {
            // Placeholder
            return new Dictionary<string, string>();
        }

        public void SaveGame(GameState gameState) {
            // Placeholder
        }

        public GameState LoadSavedGame() {
            return new GameState();
            // Placeholder
        }

        public string LoadRules() {
            // Placeholder
            return "Solitaire rules will go here.";
        }
    }
}
