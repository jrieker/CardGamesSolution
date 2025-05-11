using CardGamesSolution.Server.UserAccount;
using CardGamesSolution.Server.Database;

namespace CardGamesSolution.Tests
{
    [TestClass]
    public class UserDataAccessorTests
    {
        private IDatabaseConnection databaseConnection;
        private IUserDataAccessor userDataAccessor;

        [TestInitialize]
        public void Setup()
        {
            databaseConnection = new DatabaseConnection();
            userDataAccessor = new UserDataAccessor(databaseConnection);
        }

        [TestMethod]
        public void GetUserByUsername_ReturnsCorrectUser()
        {
            // Arrange
            string username = "testUserA";
            if (userDataAccessor.UserExists(username))
                userDataAccessor.DeleteUserByUsername(username);

            var expectedUser = new User(-1, username, "pass", 3, 2, 25.0f, 5);
            userDataAccessor.SaveUserData(expectedUser);

            // Act
            var actualUser = userDataAccessor.GetUserByUsername(username);

            // Assert
            Assert.AreEqual(expectedUser.Username, actualUser.Username);
            Assert.AreEqual(expectedUser.Password, actualUser.Password);
            Assert.AreEqual(expectedUser.Wins, actualUser.Wins);
            Assert.AreEqual(expectedUser.Losses, actualUser.Losses);
            Assert.AreEqual(expectedUser.Balance, actualUser.Balance);
            Assert.AreEqual(expectedUser.GamesPlayed, actualUser.GamesPlayed);
        }

        [TestMethod]
        public void SaveUserData_AddsUser()
        {
            // Arrange
            string username = "testUserB";
            if (userDataAccessor.UserExists(username)) {
                userDataAccessor.DeleteUserByUsername(username);
            }
            var user = new User(userDataAccessor.GetNextUserId(), username, "initial", 1, 1, 10.0f, 2);
            

            // Act
            userDataAccessor.SaveUserData(user);
            var savedUser = userDataAccessor.GetUserByUsername(username);

            // Assert
            Assert.IsNotNull(savedUser);
            Assert.AreEqual(user.UserId, savedUser.UserId);
            Assert.AreEqual(user.Username, savedUser.Username);
            Assert.AreEqual(user.Password, savedUser.Password);
            Assert.AreEqual(user.Wins, savedUser.Wins);
            Assert.AreEqual(user.Losses, savedUser.Losses);
            Assert.AreEqual(user.Balance, savedUser.Balance);
            Assert.AreEqual(user.GamesPlayed, savedUser.GamesPlayed);
        }

        [TestMethod]
        public void SaveUserData_UpdatesUser()
        {
            // Arrange
            string username = "testUserC";
            if (userDataAccessor.UserExists(username)) {
                userDataAccessor.DeleteUserByUsername(username);
            }
            var user = new User(userDataAccessor.GetNextUserId(), username, "initial", 1, 1, 10.0f, 2);
            userDataAccessor.SaveUserData(user);
            user = userDataAccessor.GetUserByUsername(username);
            user.Balance = 5.0f;

            // Act
            userDataAccessor.SaveUserData(user);
            var savedUser = userDataAccessor.GetUserByUsername(username);

            // Assert
            Assert.IsNotNull(savedUser);
            Assert.AreEqual(user.UserId, savedUser.UserId);
            Assert.AreEqual(user.Username, savedUser.Username);
            Assert.AreEqual(user.Password, savedUser.Password);
            Assert.AreEqual(user.Wins, savedUser.Wins);
            Assert.AreEqual(user.Losses, savedUser.Losses);
            Assert.AreEqual(user.Balance, savedUser.Balance);
            Assert.AreEqual(user.GamesPlayed, savedUser.GamesPlayed);
        }

        [TestMethod]
        public void UserExists_ReturnsTrue()
        {
            // Arrange
            string username = "testUserD";
            if (!userDataAccessor.UserExists(username)) {
                userDataAccessor.SaveUserData(new User(userDataAccessor.GetNextUserId(), username, "somepass"));
            }

            // Act
            bool exists = userDataAccessor.UserExists(username);

            // Assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void UserExists_ReturnsFalse()
        {
            // Arrange
            string username = "testUserE";
            if (userDataAccessor.UserExists(username)) {
                userDataAccessor.DeleteUserByUsername(username);
            }

            // Act
            bool exists = userDataAccessor.UserExists(username);

            // Assert
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void GetNextUserId_ReturnsNextId()
        {
            // Arrange 
            int id1 = userDataAccessor.GetNextUserId();
            userDataAccessor.SaveUserData(new User(id1, "idTestUser", "pass"));

            // Act
            int id2 = userDataAccessor.GetNextUserId();

            // Assert
            Assert.AreEqual(id1 + 1, id2);
        }

        [TestMethod]
        public void DeleteUserByUsername_RemovesUser()
        {
            // Arrange
            string username = "testUserDelete";
            if (!userDataAccessor.UserExists(username)) {
                userDataAccessor.SaveUserData(new User(userDataAccessor.GetNextUserId(), username, "deleteMe"));
            }
            
            // Act
            userDataAccessor.DeleteUserByUsername(username);
            bool exists = userDataAccessor.UserExists(username);

            // Assert
            Assert.IsFalse(exists);
        }
    }
}
