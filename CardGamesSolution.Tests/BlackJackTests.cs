using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardGamesSolution.Server.Blackjack;
using CardGamesSolution.Server.Shared;
using CardGamesSolution.Server.UserAccount;

namespace CardGamesSolution.Tests
{
    [TestClass]
    public class BlackJackEngineSimpleTests
    {
        private BlackJackEngine engine = null!;
        private User[] users = null!;

        [TestInitialize]
        public void Setup()
        {
            // Arrange 
            users = new[]
            {
                new User { UserId = 1, Username = "Alice", Balance = 100f },
                new User { UserId = 2, Username = "Bob",   Balance = 100f }
            };

            engine = new BlackJackEngine();
            engine.Intialize(users);
        }

        [TestMethod]
        public void InitialTurnIndex_IsZero()
        {
            // Act
            int turn = engine.GetCurrentTurnIndex();

            // Assert
            Assert.AreEqual(0, turn);
        }


        [TestMethod]
        public void ComputePayout_WorksForBustWinLosePush()
        {
            // Arrange
            float bet = 50f;

            // Player busts
            Assert.AreEqual(-bet, engine.ComputePayout(22, 10, bet));

            // Dealer busts
            Assert.AreEqual(+bet, engine.ComputePayout(20, 22, bet));

            // Player wins
            Assert.AreEqual(+bet, engine.ComputePayout(19, 17, bet));

            // Player loses
            Assert.AreEqual(-bet, engine.ComputePayout(16, 18, bet));

            // Push
            Assert.AreEqual(0f, engine.ComputePayout(18, 18, bet));
        }

        [TestMethod]
        public void Stand_AdvancesTurnIndex()
        {
            // Arrange
            // initial turn is 0
            int before = engine.GetCurrentTurnIndex();
            int playerId = users[0].UserId;

            // Act
            dynamic result = engine.Stand(playerId);

            // Assert
            Assert.IsTrue(result.success);
            
            int expected = (before + 1) % users.Length;
            Assert.AreEqual(expected, (int)result.currentTurnIndex);
        }
    }
}
