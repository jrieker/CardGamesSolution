using System;
using System.Collections.Generic;
using CardGamesSolution.Server.Shared;


namespace CardGamesSolution.Server.Solitaire {
    public class SolitaireManager : ISolitaireManager {
        private ISolitaireEngine engine;
        private Deck deck;

        public SolitaireManager(ISolitaireEngine solitaireEngine) {
            engine = solitaireEngine;
            deck = new Deck();
        }

        public void StartNewGame() {
            deck.Shuffle();
            engine.InitializeGame(deck.GetCards());

            // method for starting or restarting game
        }

        public void DealCards() {
            // Placeholder
        }

        public void Move(Card card, string fromPile, string toPile) {
            var result = engine.ProcessMove(card, fromPile, toPile);
            Console.WriteLine(result.Message);

            // calls engine to move card, engine checks if valid, result is printed to console
        }

        public void DrawFromStock() {
            var gameState = engine.GetGameState();
            if (gameState.Stock.Count > 0) {
                var card = gameState.Stock[0];
                gameState.Stock.RemoveAt(0);
                gameState.Waste.Add(card);
            } else {
                Console.WriteLine("Stock is empty.");
            }

            // if there are cards left in stock, move top one to the waste, if stock is empty, print that it is
        }

        public void Restart() {
            StartNewGame();

            // calls StartNewGame again to restart
        }

        public GameState GetGameState() {
            return engine.GetGameState();

            // gets gamestate
        }
    }
}
