using Microsoft.Data.SqlClient;
using CardGamesSolution.Server.Database;

namespace CardGamesSolution.Tests
{
    [TestClass]
    public class DatabaseConnectionTests
    {
        private IDatabaseConnection databaseConnection;

        [TestInitialize]
        public void Setup()
        {
            databaseConnection = new DatabaseConnection();
        }

        [TestMethod]
        public void GetConnection_Returns()
        {
            // Arrange
            SqlConnection connection;

            // Act
            connection = databaseConnection.GetConnection();

            // Assert
            Assert.IsNotNull(connection);
            Assert.IsInstanceOfType(connection, typeof(SqlConnection));
        }

        [TestMethod]
        public void GetConnection_CanOpenAndClose()
        {
            // Arrange
            var connection = databaseConnection.GetConnection();

            // Act
            connection.Open();
            var stateWhenOpen = connection.State;
            connection.Close();
            var stateWhenClosed = connection.State;

            // Assert
            Assert.AreEqual(System.Data.ConnectionState.Open, stateWhenOpen);
            Assert.AreEqual(System.Data.ConnectionState.Closed, stateWhenClosed);
        }
    }
}