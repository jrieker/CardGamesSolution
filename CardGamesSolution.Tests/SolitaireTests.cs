using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardGamesSolution.Server.Solitaire;
using CardGamesSolution.Server.Shared;
using System.Collections.Generic;

namespace CardGamesSolution.Tests
{
    [TestClass]
    public class SolitaireEngineTests
    {
        private SolitaireEngine engine;
        private GameState state;

        [TestInitialize]
        public void Setup()
        {
            state = new GameState();
            engine = new SolitaireEngine(state); // inject state
        }

        [TestMethod]
        public void InitializeGame_SetsTableauCorrectly()
        {
            // Arrange
            var deck = new List<Card>();
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            foreach (var suit in suits)
                for (int i = 1; i <= 13; i++)
                    deck.Add(new Card(suit, i));

            // Act
            engine.InitializeGame(deck);
            var current = engine.GetGameState();

            // Assert
            Assert.AreEqual(7, current.Tableau.Count);
            Assert.AreEqual(1, current.Tableau[0].FaceUp.Count);
        }

        [TestMethod]
        public void ProcessMove_AceToMatchingFoundation_Succeeds()
        {
            // Arrange
            var ace = new Card("Spades", 1);
            state.Waste.Add(ace);

            // Act
            var result = engine.ProcessMove(ace, "Waste", "Spades");

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(1, result.GameState.Foundation["Spades"].Count);
        }

        [TestMethod]
        public void ProcessMove_AceToWrongFoundation_Fails()
        {
            // Arrange
            var ace = new Card("Hearts", 1);
            state.Waste.Add(ace);

            // Act
            var result = engine.ProcessMove(ace, "Waste", "Spades");

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void ProcessMove_OnlyKingToEmptyPile_Succeeds()
        {
            // Arrange
            var king = new Card("Clubs", 13);
            state.Waste.Add(king);

            // Act
            var result = engine.ProcessMove(king, "Waste", "Pile1");

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void ProcessMove_QueenToEmptyPile_Fails()
        {
            // Arrange
            var queen = new Card("Hearts", 12);
            state.Waste.Add(queen);

            // Act
            var result = engine.ProcessMove(queen, "Waste", "Pile1");

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void ProcessMove_TableauToTableauValid_Succeeds()
        {
            // Arrange
            var six = new Card("Hearts", 6);
            var seven = new Card("Clubs", 7);
            state.Tableau[0].FaceUp.Add(six);
            state.Tableau[1].FaceUp.Add(seven);

            // Act
            var result = engine.ProcessMove(six, "Pile1", "Pile2");

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void CheckForWin_AllFoundationsFull_ReturnsTrue()
        {
            // Arrange
            foreach (var suit in new[] { "Hearts", "Diamonds", "Clubs", "Spades" })
                for (int i = 1; i <= 13; i++)
                    state.Foundation[suit].Add(new Card(suit, i));

            // Act
            var result = engine.CheckForWin();

            // Assert
            Assert.IsTrue(result);
        }
    }
}
