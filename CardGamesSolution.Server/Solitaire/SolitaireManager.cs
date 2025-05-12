using System;
using System.Collections.Generic;
using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Solitaire
{
    public class SolitaireManager : ISolitaireManager
    {
        private ISolitaireEngine engine;

        public SolitaireManager(ISolitaireEngine solitaireEngine)
        {
            engine = solitaireEngine;
        }

        public void StartNewGame()
        {
            var deck = new Deck();
            deck.Shuffle();
            engine.InitializeGame(deck.GetCards());
        }

        public MoveResult Move(Card card, string fromPile, string toPile)
        {
            return engine.ProcessMove(card, fromPile, toPile);
        }

        public void DrawFromStock()
        {
            var gameState = engine.GetGameState();

            if (gameState.Stock.Count > 0)
            {
                var card = gameState.Stock[0];
                gameState.Stock.RemoveAt(0);
                gameState.Waste.Add(card);
            }
            else
            {
                Console.WriteLine("Stock is empty.");
            }
        }

        public GameState GetGameState()
        {
            return engine.GetGameState();
        }
    }
}
