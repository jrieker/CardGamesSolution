using Microsoft.Data.SqlClient;

namespace CardGamesSolution.Server.Database
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly string connectionString =
            //REPLACE WITH YOUR CONNECTION STRING
            "Server=JACOBLENOVO\\SQLEXPRESS;Database=CardGames;Trusted_Connection=True;TrustServerCertificate=True;\r\n";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}