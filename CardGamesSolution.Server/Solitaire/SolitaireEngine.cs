using System;
using System.Collections.Generic;

namespace SolitaireEngine {
    public class SolitaireEngine : ISolitaireEngine {
        private GameState gameState = new GameState();

        // This is all Solitaire LOGIC
        // Gamestate holds all logic about current game
        // Gamestate updates as a player makes moves

        public void InitializeGame(List<Card> cards) {
            gameState = new GameState();

            // Starts a new game with shuffled cards

            int index = 0;
            for (int i = 0; i < 7; i++) {
                var pile = gameState.Tableau[i];
                for (int j = 0; j < i; j++) {
                    pile.FaceDown.Add(cards[index++]);
                }
                pile.FaceUp.Add(cards[index++]);
            }

            // deals cards to 7 tableau piles
            // for each pile i, put all cards faced down and 1 card faced up

            for (; index < cards.Count; index++) {
                gameState.Stock.Add(cards[index]);

                // throw all remaining cards in stock pile once tableau finished
            }
        }

        public MoveResult ProcessMove(Card card, string fromPile, string toPile) {
            TableauPile? fromPileObj = null;

            // moves card fromPile toPile
            // will return moveResult at end

            if (gameState.Foundation.ContainsKey(toPile)) {
                var foundationPile = gameState.Foundation[toPile];

                // checks if toPile is a foundation pile and gets that pile

                if (!card.Suit.Equals(toPile, StringComparison.OrdinalIgnoreCase)) {
                    return new MoveResult(false, "Card must match the foundation suit.", gameState);
                }

                // only cards of the correct suit can go into the foundation pile

                if (foundationPile.Count == 0 && card.Number != 1) {
                    return new MoveResult(false, "Only an Ace can be placed on an empty foundation pile.", gameState);
                }

                // makes sure foundation starts with an Ace

                if (foundationPile.Count > 0 && card.Number != foundationPile[^1].Number + 1) {
                    return new MoveResult(false, "Card must be one rank higher than top of foundation.", gameState);
                }

                // Checks cards is 1 number higher than the card on top of the foundation pile its going on

                if (fromPile == "Waste") {
                    if (gameState.Waste.Count == 0 || gameState.Waste[^1] != card) {
                        return new MoveResult(false, "You can only move the top card from the waste.", gameState);

                        // if card came from waste, make sure its top card and remove it
                    }
                    gameState.Waste.RemoveAt(gameState.Waste.Count - 1);
                } else if (fromPile.StartsWith("Pile")) {
                    int fromIndex = int.Parse(fromPile.Replace("Pile", "")) - 1;
                    fromPileObj = gameState.Tableau[fromIndex];

                    // checks if fromPile is one of the piles in the tableau, gets actual pile

                    if (fromPileObj.FaceUp.Count == 0 || fromPileObj.FaceUp[^1] != card) {
                        return new MoveResult(false, "You can only move the top face up card.", gameState);
                    }

                    // checks tableau pile has at least one face up card, make sure card requested is top face up card

                    fromPileObj.FaceUp.RemoveAt(fromPileObj.FaceUp.Count - 1);
                    if (fromPileObj.FaceUp.Count == 0 && fromPileObj.FaceDown.Count > 0) {
                        var flipped = fromPileObj.FaceDown[^1];
                        fromPileObj.FaceDown.RemoveAt(fromPileObj.FaceDown.Count - 1);
                        fromPileObj.FaceUp.Add(flipped);

                        // removes top card from piles face up cards
                        // if no cards are left faced up after flip top card upside down
                        // this removes it from faced down and adds it to faced up
                    }
                } else {
                    return new MoveResult(false, "Invalid from pile.", gameState);
                }

                // checks for errors

                foundationPile.Add(card);
                return new MoveResult(true, "Card moved to foundation.", gameState);

                // adds card to foundation if passes everything
            }

            if (fromPile.StartsWith("Pile")) {
                int fromIndex = int.Parse(fromPile.Replace("Pile", "")) - 1;
                fromPileObj = gameState.Tableau[fromIndex];

                // if moving card from tableau pile, get pile from gamestate

                if (fromPileObj.FaceUp.Count == 0 || fromPileObj.FaceUp[^1] != card) {
                    return new MoveResult(false, "You can only move the top face up card from a tableau pile.", gameState);

                    // only lets you move the top faced up card
                }
            } else if (fromPile == "Waste") {
                if (gameState.Waste.Count == 0 || gameState.Waste[^1] != card) {
                    return new MoveResult(false, "You can only move the top card from the waste.", gameState);

                    // if your moving a card from waste it has to be the top card
                    // if waste is empty or card doesn't match input reject
                }
            } else {
                return new MoveResult(false, "Invalid from pile.", gameState);

                // catches errors in inputs for piles
            }

            int toIndex = int.Parse(toPile.Replace("Pile", "")) - 1;
            var toPileObj = gameState.Tableau[toIndex];

            // gets the fromPile tableau pile
            // grabs the pile object from gamestate

            if (toPileObj.FaceUp.Count == 0 && toPileObj.FaceDown.Count == 0) {
                if (card.Number != 13) {
                    return new MoveResult(false, "Only a King can be placed on an empty pile.", gameState);

                    // only kings can be moved into empty piles
                    // if empty and card isn't a king reject move
                }

                if (fromPile == "Waste") {
                    gameState.Waste.RemoveAt(gameState.Waste.Count - 1);
                } else {
                    fromPileObj!.FaceUp.RemoveAt(fromPileObj.FaceUp.Count - 1);
                    if (fromPileObj.FaceUp.Count == 0 && fromPileObj.FaceDown.Count > 0) {
                        var flipped = fromPileObj.FaceDown[^1];
                        fromPileObj.FaceDown.RemoveAt(fromPileObj.FaceDown.Count - 1);
                        fromPileObj.FaceUp.Add(flipped);
                    }
                }

                // remove card from pile it came from either waste or tableau, flip card if needed
                // This logic will also appear later, one if for moving K to empty space, other is for moving card to non empty space

                toPileObj.FaceUp.Add(card);
                return new MoveResult(true, "King moved to empty pile.", gameState);

                // adds king to a empty pile
            }

            Card destCard = toPileObj.FaceUp[^1];
            if (card.Number != destCard.Number - 1) {
                return new MoveResult(false, "Card must be one rank lower than destination.", gameState);
            }

            // standard logic for moving card on top of another (number)

            if (card.IsRed() == destCard.IsRed()) {
                return new MoveResult(false, "Card must be opposite color of destination.", gameState);
            }

            // standard logic for moving card on top of another (rank)

            if (fromPile == "Waste") {
                gameState.Waste.RemoveAt(gameState.Waste.Count - 1);
            } else {
                fromPileObj!.FaceUp.RemoveAt(fromPileObj.FaceUp.Count - 1);
                if (fromPileObj.FaceUp.Count == 0 && fromPileObj.FaceDown.Count > 0) {
                    var flipped = fromPileObj.FaceDown[^1];
                    fromPileObj.FaceDown.RemoveAt(fromPileObj.FaceDown.Count - 1);
                    fromPileObj.FaceUp.Add(flipped);

                    // have this above also, this is the removal and flip again, but for the standard moves
                }
            }

            toPileObj.FaceUp.Add(card);

            // adds card to new Pile

            return new MoveResult(true, "Move successful.", gameState);
            
            // return succesful if the move is valid
        }

        public bool CheckForWin() {
            foreach (var foundation in gameState.Foundation.Values) {
                if (foundation.Count != 13) {
                    return false;
                }
            }
            return true;

            // goes thru each foundation pile, if any pile does not have 13 cards, return false
            // if all Piles have 13 and have all A-K return true
        }

        public GameState GetGameState() {
            return gameState;

            // shows current gamestate
        }
    }
}
