using Microsoft.Data.SqlClient;
using CardGamesSolution.Server.Database;
using System;
using System.Collections.Generic;

namespace CardGamesSolution.Server.Leaderboard
{
    public class LeaderboardAccessor : ILeaderBoardAccessor
    {
        private readonly List<LeaderboardEntry> leaderboardData = new List<LeaderboardEntry>();

        public void GetConnection()
        {
            using SqlConnection conn = databaseConnection.getConnection();
            conn.Open();
            Console.WriteLine("Connection opened successfully.");
        }

        public void UpdateLeaderBoard()
        {
            leaderboardData.Clear();

            using SqlConnection conn = databaseConnection.getConnection();
            conn.Open();

            string query = "SELECT * FROM Leaderboard";
            using SqlCommand cmd = new SqlCommand(query, conn);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                leaderboardData.Add(new LeaderboardEntry
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Username = reader["Username"].ToString(),
                    Wins = Convert.ToInt32(reader["Wins"]),
                    Losses = Convert.ToInt32(reader["Losses"]),
                    WinStreak = Convert.ToInt32(reader["WinStreak"]),
                    Balance = Convert.ToInt32(reader["Balance"]),
                    GamesPlayed = Convert.ToInt32(reader["GamesPlayed"])
                });
            }
        }

        public void SaveLeaderBoardData()
        {
            using SqlConnection conn = databaseConnection.getConnection();
            conn.Open();

            foreach (var entry in leaderboardData)
            {
                string query = @"
                    INSERT INTO Leaderboard (Id, Username, Wins, Losses, WinStreak, Balance, GamesPlayed) 
                    VALUES (@Id, @Username, @Wins, @Losses, @WinStreak, @Balance, @GamesPlayed)";

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", entry.Id);
                cmd.Parameters.AddWithValue("@Username", entry.Username);
                cmd.Parameters.AddWithValue("@Wins", entry.Wins);
                cmd.Parameters.AddWithValue("@Losses", entry.Losses);
                cmd.Parameters.AddWithValue("@WinStreak", entry.WinStreak);
                cmd.Parameters.AddWithValue("@Balance", entry.Balance);
                cmd.Parameters.AddWithValue("@GamesPlayed", entry.GamesPlayed);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateLeaderBoardData()
        {
            using SqlConnection conn = databaseConnection.getConnection();
            conn.Open();

            foreach (var entry in leaderboardData)
            {
                string query = @"
                    UPDATE Leaderboard 
                    SET Wins = @Wins, Losses = @Losses, WinStreak = @WinStreak, 
                        Balance = @Balance, GamesPlayed = @GamesPlayed
                    WHERE Id = @Id";

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", entry.Id);
                cmd.Parameters.AddWithValue("@Wins", entry.Wins);
                cmd.Parameters.AddWithValue("@Losses", entry.Losses);
                cmd.Parameters.AddWithValue("@WinStreak", entry.WinStreak);
                cmd.Parameters.AddWithValue("@Balance", entry.Balance);
                cmd.Parameters.AddWithValue("@GamesPlayed", entry.GamesPlayed);
                cmd.ExecuteNonQuery();
            }
        }

        public void SortLeaderBoard()
        {
            // You can change this to sort by Balance, WinStreak, etc.
            leaderboardData.Sort((a, b) => b.Wins.CompareTo(a.Wins));
        }

        public int GetNextLeaderBoardId()
        {
            int maxId = 0;
            using SqlConnection conn = databaseConnection.getConnection();
            conn.Open();

            string query = "SELECT ISNULL(MAX(Id), 0) FROM Leaderboard";
            using SqlCommand cmd = new SqlCommand(query, conn);
            object result = cmd.ExecuteScalar();

            if (result != DBNull.Value)
                maxId = Convert.ToInt32(result);

            return maxId + 1;
        }
    }

    public class LeaderboardEntry
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int WinStreak { get; set; }
        public int Balance { get; set; }
        public int GamesPlayed { get; set; }
    }
}
