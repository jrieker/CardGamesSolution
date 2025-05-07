using System;
using System.Collections.Generic;
using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Solitaire {
    public class SolitaireEngine : ISolitaireEngine {
        private GameState gameState = new GameState();

        public SolitaireEngine() {
            gameState = new GameState();
        }

        public SolitaireEngine(GameState initialState) {
            gameState = initialState;
        }

        public void SetGameState(GameState newState) {
            gameState = newState;
        }

        public void InitializeGame(List<Card> cards) {
            gameState = new GameState();
            int index = 0;

            for (int i = 0; i < 7; i++) {
                var pile = gameState.Tableau[i];
                for (int j = 0; j < i; j++) {
                    pile.FaceDown.Add(cards[index++]);
                }
                pile.FaceUp.Add(cards[index++]);
            }

            for (; index < cards.Count; index++) {
                gameState.Stock.Add(cards[index]);
            }
        }

        public MoveResult ProcessMove(Card card, string fromPile, string toPile) {
            TableauPile? fromPileObj = null;

            if (gameState.Foundation.ContainsKey(toPile)) {
                var foundationPile = gameState.Foundation[toPile];

                if (!card.Suit.Equals(toPile, StringComparison.OrdinalIgnoreCase)) {
                    return new MoveResult(false, "Card must match the foundation suit.", gameState);
                }

                if (foundationPile.Count == 0 && card.Number != 1) {
                    return new MoveResult(false, "Only an Ace can be placed on an empty foundation pile.", gameState);
                }

                if (foundationPile.Count > 0 && card.Number != foundationPile[^1].Number + 1) {
                    return new MoveResult(false, "Card must be one rank higher than top of foundation.", gameState);
                }

                if (fromPile == "Waste") {
                    if (gameState.Waste.Count == 0 || !CardsEqual(gameState.Waste[^1], card)) {
                        return new MoveResult(false, "You can only move the top card from the waste.", gameState);
                    }
                    gameState.Waste.RemoveAt(gameState.Waste.Count - 1);
                } else if (fromPile.StartsWith("Pile")) {
                    int fromIndex = int.Parse(fromPile.Replace("Pile", "")) - 1;
                    fromPileObj = gameState.Tableau[fromIndex];
                    if (fromPileObj.FaceUp.Count == 0 || !CardsEqual(fromPileObj.FaceUp[^1], card)) {
                        return new MoveResult(false, "You can only move the top face up card.", gameState);
                    }
                    fromPileObj.FaceUp.RemoveAt(fromPileObj.FaceUp.Count - 1);
                    if (fromPileObj.FaceUp.Count == 0 && fromPileObj.FaceDown.Count > 0) {
                        var flipped = fromPileObj.FaceDown[^1];
                        fromPileObj.FaceDown.RemoveAt(fromPileObj.FaceDown.Count - 1);
                        fromPileObj.FaceUp.Add(flipped);
                    }
                } else {
                    return new MoveResult(false, "Invalid from pile.", gameState);
                }

                foundationPile.Add(card);
                return new MoveResult(true, "Card moved to foundation.", gameState);
            }

            if (fromPile.StartsWith("Pile")) {
                int fromIndex = int.Parse(fromPile.Replace("Pile", "")) - 1;
                if (fromIndex < 0 || fromIndex >= gameState.Tableau.Count)
                    return new MoveResult(false, "Invalid fromPile index.", gameState);
                fromPileObj = gameState.Tableau[fromIndex];
            } else if (fromPile == "Waste") {
                if (gameState.Waste.Count == 0 || !CardsEqual(gameState.Waste[^1], card)) {
                    return new MoveResult(false, "You can only move the top card from the waste.", gameState);
                }
            } else {
                return new MoveResult(false, "Invalid from pile.", gameState);
            }

            int toIndex = int.Parse(toPile.Replace("Pile", "")) - 1;
            if (toIndex < 0 || toIndex >= gameState.Tableau.Count)
                return new MoveResult(false, "Invalid toPile index.", gameState);
            var toPileObj = gameState.Tableau[toIndex];

            if (toPileObj.FaceUp.Count == 0 && toPileObj.FaceDown.Count == 0) {
                if (card.Number != 13) {
                    return new MoveResult(false, "Only a King can be placed on an empty pile.", gameState);
                }

                if (fromPile == "Waste") {
                    gameState.Waste.RemoveAt(gameState.Waste.Count - 1);
                    toPileObj.FaceUp.Add(card);
                } else {
                    int moveIndex = fromPileObj.FaceUp.FindIndex(c => CardsEqual(c, card));
                    if (moveIndex == -1)
                        return new MoveResult(false, "Card not found in source.", gameState);
                    var stack = fromPileObj.FaceUp.GetRange(moveIndex, fromPileObj.FaceUp.Count - moveIndex);
                    fromPileObj.FaceUp.RemoveRange(moveIndex, stack.Count);
                    if (fromPileObj.FaceUp.Count == 0 && fromPileObj.FaceDown.Count > 0) {
                        var flipped = fromPileObj.FaceDown[^1];
                        fromPileObj.FaceDown.RemoveAt(fromPileObj.FaceDown.Count - 1);
                        fromPileObj.FaceUp.Add(flipped);
                    }
                    toPileObj.FaceUp.AddRange(stack);
                }

                return new MoveResult(true, "King moved to empty pile.", gameState);
            }

            if (toPileObj.FaceUp.Count == 0) {
                return new MoveResult(false, "No cards to stack onto.", gameState);
            }

            Card destCard = toPileObj.FaceUp[^1];
            if (card.Number != destCard.Number - 1) {
                return new MoveResult(false, "Card must be one rank lower than destination.", gameState);
            }
            if (card.IsRed() == destCard.IsRed()) {
                return new MoveResult(false, "Card must be opposite color of destination.", gameState);
            }

            if (fromPile == "Waste") {
                gameState.Waste.RemoveAt(gameState.Waste.Count - 1);
                toPileObj.FaceUp.Add(card);
            } else {
                int moveIndex = fromPileObj.FaceUp.FindIndex(c => CardsEqual(c, card));
                if (moveIndex == -1)
                    return new MoveResult(false, "Card not found in source tableau.", gameState);
                var movingStack = fromPileObj.FaceUp.GetRange(moveIndex, fromPileObj.FaceUp.Count - moveIndex);
                fromPileObj.FaceUp.RemoveRange(moveIndex, movingStack.Count);
                if (fromPileObj.FaceUp.Count == 0 && fromPileObj.FaceDown.Count > 0) {
                    var flipped = fromPileObj.FaceDown[^1];
                    fromPileObj.FaceDown.RemoveAt(fromPileObj.FaceDown.Count - 1);
                    fromPileObj.FaceUp.Add(flipped);
                }
                toPileObj.FaceUp.AddRange(movingStack);
            }

            return new MoveResult(true, "Move successful.", gameState);
        }

        private bool CardsEqual(Card a, Card b) {
            return a.Suit == b.Suit && a.Number == b.Number;
        }

        public bool CheckForWin() {
            foreach (var foundation in gameState.Foundation.Values) {
                if (foundation.Count != 13) {
                    return false;
                }
            }
            return true;
        }

        public GameState GetGameState() {
            return gameState;
        }
    }
}
