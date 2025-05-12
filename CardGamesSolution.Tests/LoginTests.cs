using CardGamesSolution.Server.UserAccount;
using CardGamesSolution.Server.Database;

namespace CardGamesSolution.Tests
{
    [TestClass]
    public class LoginManagerTests
    {
        private IDatabaseConnection databaseConnection;
        private IUserDataAccessor userDataAccessor;
        private LoginEngine loginEngine;
        private ILoginManager loginManager;

        [TestInitialize]
        public void Setup()
        {
            databaseConnection = new DatabaseConnection();
            userDataAccessor = new UserDataAccessor(databaseConnection);
            loginEngine = new LoginEngine(userDataAccessor);
            loginManager = new LoginManager(loginEngine);
        }

        [TestMethod]
        public void Register_NewUser()
        {
            // Arrange
            string username = "newUser";
            string password = "securePass";
            if (userDataAccessor.UserExists(username)) 
            {
                userDataAccessor.DeleteUserByUsername(username);
            }

            // Act
            loginManager.Register(username, password);
            var savedUser = userDataAccessor.GetUserByUsername(username);

            // Assert
            Assert.IsNotNull(savedUser);
            Assert.AreEqual(username, savedUser.Username);
            Assert.AreEqual(password, savedUser.Password);
        }

        [TestMethod]
        public void Register_ExistingUser()
        {
            // Arrange
            string username = "newUser999";
            userDataAccessor.SaveUserData(new User(1, username, "pass123"));

            // Act
            void action() => loginManager.Register(username, "newpass");

            // Assert
            var exception = Assert.ThrowsException<Exception>(action);
            Assert.AreEqual("User already exists!", exception.Message);
        }

        [TestMethod]
        public void Login_ReturnsUser()
        {
            // Arrange
            int userId = -1;
            string username = "testUser";
            string password = "securePass";
            int wins = 2;
            int losses = 1;
            float balance = 100.0f;
            int gamesPlayed = 3;

            if (userDataAccessor.UserExists(username)) 
            {
                userDataAccessor.DeleteUserByUsername(username);
            }
            User user = new User(userId, username, password, wins, losses, balance, gamesPlayed);
            userDataAccessor.SaveUserData(user);

            // Act
            var result = loginManager.Login(username, password);

            // Assert
            Assert.AreEqual(user.UserId, result.UserId);
            Assert.AreEqual(user.Username, result.Username);
            Assert.AreEqual(user.Password, result.Password);
            Assert.AreEqual(user.Wins, result.Wins);
            Assert.AreEqual(user.Losses, result.Losses);
            Assert.AreEqual(user.Balance, result.Balance);
            Assert.AreEqual(user.GamesPlayed, result.GamesPlayed);
        }

        [TestMethod]
        public void Login_WrongPassword()
        {
            // Arrange
            string username = "newUser";

            if (userDataAccessor.UserExists(username)) 
            {
                userDataAccessor.DeleteUserByUsername(username);
            }
            userDataAccessor.SaveUserData(new User(-2, username, "correctpass"));

            // Act
            void action() => loginManager.Login(username, "wrongpass");

            // Assert
            var exception = Assert.ThrowsException<Exception>(action);
            Assert.AreEqual("Incorrect password!", exception.Message);
        }

        [TestMethod]
        public void Login_UserNotFound()
        {
            // Arrange
            string username = "newUser";
            if (userDataAccessor.UserExists(username)) 
            {
                userDataAccessor.DeleteUserByUsername(username);
            }

            // Act
            void action() => loginManager.Login(username, "HiEveryone");

            // Assert
            var exception = Assert.ThrowsException<Exception>(action);
            Assert.AreEqual("Username not found!", exception.Message);
        }
    }
}
