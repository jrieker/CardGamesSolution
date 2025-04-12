using Microsoft.Data.SqlClient;

namespace CardGamesSolution.Server.UserAccount
{
    public class DatabaseConnection
    {
        public static SqlConnection GetDatabaseConnection()
        {
            string connString="Data Source=localhost,1433;Initial Catalog=master;User ID=sa;Password=SuperSecure123;TrustServerCertificate=true";
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            return conn;
        }
        public static User GetUserFromUsername(string username)
        {
            User user;
            SqlConnection conn = GetDatabaseConnection();
            string query = "SELECT * FROM USERS WHERE userName = @UserName";
            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@UserName", username);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userId = (int)reader["UserId"];
                        string password = reader["passWord"].ToString() ?? string.Empty;
                        int wins = (int)reader["wins"];
                        int losses = (int)reader["losses"];
                        float balance = Convert.ToSingle(reader["balance"]);
                        user = new User(userId, username, password, wins, losses, balance);
                        conn.Close();
                    }
                    else 
                    {
                        conn.Close();
                        throw new Exception("Username not found!");
                    }
                }
            }
            return user;
        }
    }
    
}