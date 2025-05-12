using Microsoft.Data.SqlClient;
using CardGamesSolution.Server.Database;

namespace CardGamesSolution.Server.UserAccount
{
    public class UserDataAccessor : IUserDataAccessor
    {
        private IDatabaseConnection databaseConnection;

        public UserDataAccessor(IDatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public User GetUserByUsername(string username)
        {
            User user;
            using (SqlConnection conn = databaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM USERS WHERE userName = @userName";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@userName", username);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        int userId = (int)reader["UserId"];
                        string password = reader["passWord"].ToString() ?? string.Empty;
                        int wins = (int)reader["wins"];
                        int losses = (int)reader["losses"];
                        float balance = Convert.ToSingle(reader["balance"]);
                        int gamesPlayed = (int)reader["gamesPlayed"];
                        user = new User(userId, username, password, wins, losses, balance, gamesPlayed);
                    }
                }
            }
            return user;
        }

        public void SaveUserData(User user)
        {
            using (SqlConnection conn = databaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"MERGE INTO USERS AS target
                                USING (SELECT @UserId AS UserId) AS source
                                        ON target.UserId = source.UserId
                                        WHEN MATCHED THEN
                                            UPDATE SET 
                                                userName = @userName,
                                                passWord = @passWord,
                                                balance = @balance,
                                                wins = @wins,
                                                losses = @losses,
                                                gamesPlayed = @gamesPlayed
                                        WHEN NOT MATCHED THEN
                                            INSERT (UserId, userName, passWord, balance, wins, losses, gamesPlayed)
                                            VALUES (@UserId, @userName, @passWord, @balance, @wins, @losses, @gamesPlayed);";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@UserId", user.UserId);
                    command.Parameters.AddWithValue("@userName", user.Username);
                    command.Parameters.AddWithValue("@passWord", user.Password);
                    command.Parameters.AddWithValue("@balance", user.Balance);
                    command.Parameters.AddWithValue("@wins", user.Wins);
                    command.Parameters.AddWithValue("@losses", user.Losses);
                    command.Parameters.AddWithValue("@gamesPlayed", user.GamesPlayed);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool UserExists(string username)
        {
            using (SqlConnection conn = databaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(1) FROM USERS WHERE userName = @userName";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@userName", username);
                    int result = (int)command.ExecuteScalar();
                    return result > 0;
                }
            }
        }

        public int GetNextUserId()
        {
            using (SqlConnection conn = databaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT ISNULL(MAX(UserID), 0) + 1 FROM USERS";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    int nextId = (int)command.ExecuteScalar();
                    return nextId;
                }
                
            }
        }

        public void DeleteUserByUsername(string username)
        {
            using (SqlConnection conn = databaseConnection.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM USERS WHERE userName = @userName";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@userName", username);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}