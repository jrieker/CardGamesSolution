using CardGamesSolution.Server.UserAccount;

namespace CardGamesSolution.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void Constructor_BasicParams()
        {
            // Arrange
            string username = "testUser";
            string password = "testPass";

            // Act
            User user = new User(7, username, password);

            // Assert
            Assert.AreEqual(7, user.UserId);
            Assert.AreEqual(username, user.Username);
            Assert.AreEqual(password, user.Password);
            Assert.AreEqual(0, user.Wins);
            Assert.AreEqual(0, user.Losses);
            Assert.AreEqual(0f, user.Balance);
            Assert.AreEqual(0, user.GamesPlayed);
        }

        [TestMethod]
        public void Constructor_FullParams()
        {
            // Arrange
            int userId = 8;
            string username = "existingUser";
            string password = "pass123";
            int wins = 5;
            int losses = 3;
            float balance = 200;
            int gamesPlayed = 15;

            // Act
            User user = new User(userId, username, password, wins, losses, balance, gamesPlayed);

            // Assert
            Assert.AreEqual(userId, user.UserId);
            Assert.AreEqual(username, user.Username);
            Assert.AreEqual(password, user.Password);
            Assert.AreEqual(wins, user.Wins);
            Assert.AreEqual(losses, user.Losses);
            Assert.AreEqual(balance, user.Balance);
            Assert.AreEqual(15, user.GamesPlayed);
        }

        [TestMethod]
        public void UpdateBalance_Sets()
        {
            // Arrange
            User user = new User(1, "test", "pass");
            float amount = 50.75f;

            // Act
            user.UpdateBalance(amount);

            // Assert
            Assert.AreEqual(50.75f, user.Balance);
        }

        [TestMethod]
        public void UpdateBalance_Adds()
        {
            // Arrange
            User user = new User(0, "test", "pass", 0, 0, 15.5f, 0);
            float amount = 20.5f;

            // Act
            user.UpdateBalance(amount);

            // Assert
            Assert.AreEqual(36f, user.Balance);
        }

        [TestMethod]
        public void UpdateBalance_Subtracts()
        {
            // Arrange
            User user = new User(0, "test", "pass", 0, 0, 12.2f, 0);
            float amount = -1.3f;

            // Act
            user.UpdateBalance(amount);

            // Assert
            Assert.AreEqual(10.9f, user.Balance);
        }

        [TestMethod]
        public void AddWin_IncrementsWins()
        {
            // Arrange
            User user = new User(0, "test", "pass");
            user.Wins = 2;

            // Act
            user.AddWin();

            // Assert
            Assert.AreEqual(3, user.Wins);
        }

        [TestMethod]
        public void AddLoss_IncrementsLosses()
        {
            // Arrange
            User user = new User(0, "test", "pass");
            user.Losses = 1;

            // Act
            user.AddLoss();

            // Assert
            Assert.AreEqual(2, user.Losses);
        }
    }
}