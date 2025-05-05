namespace CardGamesSolution.Server.Leaderboard
{
    using Microsoft.Data.SqlClient;
    using CardGamesSolution.Server.Database;
    using System;
    using System.Collections.Generic;
    using global::CardGamesSolution.Server.Database;

    namespace CardGamesSolution.Server.Leaderboard
    {
        public class Leaderboard : ILeaderboard
        {
            private readonly IDatabaseConnection _db;

            // Inject the database connection (existing DatabaseConnection class).
            public Leaderboard(IDatabaseConnection db)
            {
                _db = db;
            }

            // Get total wins for each player across all games.
            public List<LeaderboardEntry> GetAllGameWins()
            {
                var results = new List<LeaderboardEntry>();
                using SqlConnection conn = _db.GetConnection();
                conn.Open();
                // LeaderBoard table holds each user’s overall wins (Wins column):contentReference[oaicite:14]{index=14}.
                string query = @"
                SELECT U.userName AS Username, L.Wins
                FROM LeaderBoard L
                JOIN Users U ON L.UserId = U.UserID";
                using SqlCommand cmd = new SqlCommand(query, conn);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new LeaderboardEntry
                    {
                        Username = reader.GetString(0),
                        Wins = reader.GetInt32(1)
                    });
                }
                return results;
            }

            // Get wins per player for a specific game type.
            public List<LeaderboardEntry> GetSpecificGameWins(string gameType)
            {
                var results = new List<LeaderboardEntry>();
                using SqlConnection conn = _db.GetConnection();
                conn.Open();

                // Choose the table based on gameType. For example, "Poker" or "Solitaire".
                // Each game-specific table has a Wins column and links to Users via LeaderBoardID.
                string query = "";
                if (gameType.Equals("Poker", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"
                    SELECT U.userName AS Username, P.Wins
                    FROM Poker P
                    JOIN LeaderBoard L ON P.LeaderboardID = L.LeaderBoardID
                    JOIN Users U ON L.UserId = U.UserID";
                }
                else if (gameType.Equals("Solitaire", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"
                    SELECT U.userName AS Username, S.Wins
                    FROM Solitaire S
                    JOIN LeaderBoard L ON S.LeaderboardID = L.LeaderBoardID
                    JOIN Users U ON L.UserId = U.UserID";
                }
                else
                {
                    throw new ArgumentException($"Unknown game type: {gameType}");
                }

                using SqlCommand cmd = new SqlCommand(query, conn);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new LeaderboardEntry
                    {
                        Username = reader.GetString(0),
                        Wins = reader.GetInt32(1)
                    });
                }
                return results;
            }

            // Get total earnings (balance) for each player across all games.
            public List<LeaderboardEntry> GetAllGameEarnings()
            {
                var results = new List<LeaderboardEntry>();
                using SqlConnection conn = _db.GetConnection();
                conn.Open();
                // LeaderBoard table’s Balance holds overall earnings:contentReference[oaicite:15]{index=15}.
                string query = @"
                SELECT U.userName AS Username, L.Balance
                FROM LeaderBoard L
                JOIN Users U ON L.UserId = U.UserID";
                using SqlCommand cmd = new SqlCommand(query, conn);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new LeaderboardEntry
                    {
                        Username = reader.GetString(0),
                        Balance = reader.GetInt32(1)
                    });
                }
                return results;
            }

            // Get earnings per player for a specific game type.
            public List<LeaderboardEntry> GetSpecificGameEarnings(string gameType)
            {
                var results = new List<LeaderboardEntry>();
                using SqlConnection conn = _db.GetConnection();
                conn.Open();

                string query = "";
                if (gameType.Equals("Poker", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"
                    SELECT U.userName AS Username, P.Balance
                    FROM Poker P
                    JOIN LeaderBoard L ON P.LeaderboardID = L.LeaderBoardID
                    JOIN Users U ON L.UserId = U.UserID";
                }
                else if (gameType.Equals("Solitaire", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"
                    SELECT U.userName AS Username, S.Balance
                    FROM Solitaire S
                    JOIN LeaderBoard L ON S.LeaderboardID = L.LeaderBoardID
                    JOIN Users U ON L.UserId = U.UserID";
                }
                else
                {
                    throw new ArgumentException($"Unknown game type: {gameType}");
                }

                using SqlCommand cmd = new SqlCommand(query, conn);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new LeaderboardEntry
                    {
                        Username = reader.GetString(0),
                        Balance = reader.GetInt32(1)
                    });
                }
                return results;
            }

            // Get player rankings sorted by a chosen field (e.g. "Wins" or "Balance").
            public List<LeaderboardEntry> GetPlayerRankings(string sortBy)
            {
                var results = new List<LeaderboardEntry>();
                using SqlConnection conn = _db.GetConnection();
                conn.Open();

                // Validate sortBy column to prevent SQL injection.
                string column = sortBy switch
                {
                    "Wins" => "L.Wins",
                    "Balance" => "L.Balance",
                    _ => throw new ArgumentException($"Invalid sort column: {sortBy}")
                };

                string query = $@"
                SELECT U.userName AS Username, {column} AS Value
                FROM LeaderBoard L
                JOIN Users U ON L.UserId = U.UserID
                ORDER BY {column} DESC";

                using SqlCommand cmd = new SqlCommand(query, conn);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    // Use the 'Value' column for both wins and balance, depending on sortBy.
                    var entry = new LeaderboardEntry
                    {
                        Username = reader.GetString(0)
                    };
                    if (sortBy == "Wins")
                        entry.Wins = reader.GetInt32(1);
                    else if (sortBy == "Balance")
                        entry.Balance = reader.GetInt32(1);
                    results.Add(entry);
                }
                return results;
            }
        }
    }

}
