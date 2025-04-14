using Microsoft.Data.SqlClient;

namespace CardGamesSolution.Server.Database
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly string connectionString =
            //REPLACE WITH YOUR CONNECTION STRING
            "Data Source=localhost,1433;Initial Catalog=master;User ID=sa;Password=SuperSecure123;TrustServerCertificate=true";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}