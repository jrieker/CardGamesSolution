using System;
using System.Collections.Generic;
using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Solitaire
{
    public class SolitaireManager : ISolitaireManager
    {
        private ISolitaireEngine engine;
        private Deck deck;

        public SolitaireManager(ISolitaireEngine solitaireEngine)
        {
            engine = solitaireEngine;
            deck = new Deck();
        }

        public void StartNewGame()
        {
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

        public void Restart()
        {
            StartNewGame();
        }

        public GameState GetGameState()
        {
            return engine.GetGameState();
        }

    }
}
