using System;
using System.Collections.Generic;
using SolitaireEngine;

class Program {
    static void Main() {
        // Setup
        ISolitaireEngine engine = new SolitaireEngine.SolitaireEngine();
        ISolitaireManager manager = new SolitaireManager(engine);

        // Shuffles Deck and deals Initial Cards
        manager.StartNewGame();
        GameState gameState = manager.GetGameState();

        // Initial display
        PrintGame(gameState);

        // Game loop
        while (true) {
            Console.Write("\nEnter a move: Example: 'move A of Hearts from Pile3 to Pile5', or type 'draw' or 'exit': ");
            string input = Console.ReadLine();

            // tell user to enter in a move

            if (string.IsNullOrWhiteSpace(input)) continue;
            if (input.Trim().ToLower() == "exit") break;

            // if player enenters nothing restart loop, if player exits end game

            if (input.Trim().ToLower() == "draw") {
                manager.DrawFromStock();
                gameState = manager.GetGameState();
                Console.WriteLine("Drew card from stock to waste.");
                PrintGame(gameState);
                continue;
            }

            // if a player says draw, pull a card from stock and replace it on waste pile
            // also reprints game board after

            try {
                var parts = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 8 || parts[0].ToLower() != "move" || parts[4].ToLower() != "from" || parts[6].ToLower() != "to") {
                    Console.WriteLine("Invalid format. Try: move A of Hearts from Pile3 to Pile5");
                    continue;
                }

                // Splits the String into words and makes sure input follows exact format
                // format is 8 words and it includes move from and to

                string rank = parts[1];
                string suit = parts[3];
                string fromPile = parts[5];
                string toPile = parts[7];

                // gets card info
                // which card you want to move 
                // where its coming from
                // where its going

                int number = rank switch {
                    "A" => 1,
                    "J" => 11,
                    "Q" => 12,
                    "K" => 13,
                    _ => int.TryParse(rank, out int val) ? val : -1
                };

                // converts card text into integers

                if (number < 1 || number > 13) {
                    Console.WriteLine("Invalid card rank.");
                    continue;
                }

                // checks boundaries of cards

                Card? cardToMove = null;

                if (fromPile.StartsWith("Pile")) {
                    int fromIndex = int.Parse(fromPile.Replace("Pile", "")) - 1;
                    var pile = gameState.Tableau[fromIndex];
                    cardToMove = pile.FaceUp.Find(c => c.Number == number && c.Suit.Equals(suit, StringComparison.OrdinalIgnoreCase));
                
                // gets correct pile index and looks for a face up card in that file matching numnber and suit
                
                } else if (fromPile == "Waste") {
                    if (gameState.Waste.Count > 0 && gameState.Waste[^1].Number == number &&
                        gameState.Waste[^1].Suit.Equals(suit, StringComparison.OrdinalIgnoreCase)) {
                        cardToMove = gameState.Waste[^1];
                    }
                }

                // if not in tableau, this checks if the top card on the waste is the one the player wanted to move

                if (cardToMove == null) {
                    Console.WriteLine("Card not found or is not valid.");
                    continue;
                }

                // if card is not found, dont let the move occur

                manager.Move(cardToMove, fromPile, toPile);
                gameState = manager.GetGameState();
                PrintGame(gameState);
                
                // has manager move the card if found, then refreshes and prints game again

            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");

                // if theres an error print error and re-prompt
            }
        }
    }

    static void PrintGame(GameState gameState) {
        Console.WriteLine("\nTABLEAU:");
        for (int i = 0; i < gameState.Tableau.Count; i++) {
            Console.Write($"Pile {i + 1}: ");
            foreach (var card in gameState.Tableau[i].FaceDown) {
                Console.Write("[X] ");
            }
            foreach (var card in gameState.Tableau[i].FaceUp) {
                Console.Write($"{card}, ");
            }
            Console.WriteLine();
        }

        // this is the initial game loop
        // this prints the tableau formatted correctly

        Console.WriteLine("\nFOUNDATION:");
        foreach (var suitKey in gameState.Foundation.Keys) {
            var foundationPile = gameState.Foundation[suitKey];
            if (foundationPile.Count > 0) {
                Console.WriteLine($"  {suitKey}: {foundationPile[^1]}");
            } else {
                Console.WriteLine($"  {suitKey}: Empty");
            }
        }

        // This prints the foundation formatted correctly

        Console.WriteLine("\nSTOCK:");
        Console.WriteLine($"  {gameState.Stock.Count} cards remaining.");

        // This prints the stock formatted correctly

        Console.WriteLine("\nWASTE:");
        if (gameState.Waste.Count == 0)
            Console.WriteLine("  Empty");
        else
            Console.WriteLine($"  Top card: {gameState.Waste[^1]}");

            // this prints the waste formatted correctly
    }
}