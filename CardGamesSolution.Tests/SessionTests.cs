using CardGamesSolution.Server.UserAccount;
using CardGamesSolution.Server.Database;

namespace CardGamesSolution.Tests
{
    [TestClass]
    public class SessionTests
    {
        private IUserDataAccessor userDataAccessor;
        private Session session;

        [TestInitialize]
        public void Setup()
        {
            IDatabaseConnection databaseConnection = new DatabaseConnection(); 
            userDataAccessor = new UserDataAccessor(databaseConnection);
            session = new Session(userDataAccessor);
        }

        [TestMethod]
        public void AddUser_UserIsAdded()
        {
            // Arrange
            var user = new User(1, "test", "pass");

            // Act
            session.AddUser(user);

            // Assert
            Assert.AreEqual(1, session.users.Count);
            Assert.IsTrue(session.users.Contains(user));
        }

        [TestMethod]
        public void RemoveUser_UserIsRemoved()
        {
            // Arrange
            var user = new User(2, "deleteMe", "pass");
            session.AddUser(user);

            // Act
            session.RemoveUser(user);

            // Assert
            Assert.AreEqual(0, session.users.Count);
            Assert.IsFalse(session.users.Contains(user));
        }

        [TestMethod]
        public void RemoveUser_DoesNotThrow_WhenUserNotInList()
        {
            // Arrange
            var user = new User(3, "ghost", "pass");

            // Act
            session.RemoveUser(user);
            
             //Assert
            Assert.AreEqual(0, session.users.Count);
        }
    }
}