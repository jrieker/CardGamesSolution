namespace CardGamesSolution.Server.Leaderboard
{
    using Microsoft.Data.SqlClient;
    using System;
    using System.Collections.Generic;
    using global::CardGamesSolution.Server.Database;
    using System.Data.Common;

    namespace CardGamesSolution.Server.Leaderboard
    {
        public class Leaderboard : ILeaderboard
        {
            private readonly IDatabaseConnection database;

            public Leaderboard(IDatabaseConnection dbConnection)
            {
                this.database = dbConnection;
            }

            // Get total wins for each player across all games.
            public async Task<List<LeaderboardEntry>> GetAllGameWinsAsync()
            {
                const string sql = @"
                SELECT U.UserName, L.Wins, L.Losses, L.WinStreak, L.Balance, L.GamesPlayed
                FROM LeaderBoard L
                JOIN Users U ON L.UserId = U.UserID";
                return await QueryAsync(sql);
            }

            // Get wins per player for a specific game type.
            public async Task<List<LeaderboardEntry>> GetSpecificGameWinsAsync(string gameType)
            {
                const string sqlTemplate = @"
                SELECT U.UserName, L.Wins, L.Losses, L.WinStreak, L.Balance, L.GamesPlayed
                FROM LeaderBoard L
                JOIN Users U ON L.UserId = U.UserID
                WHERE L.GameType = @GameType";
                return await QueryAsync(sqlTemplate, cmd => cmd.Parameters.AddWithValue("@GameType", gameType));
            }

            // Get total earnings (balance) for each player across all games.
            public async Task<List<LeaderboardEntry>> GetAllGameEarningsAsync()
            {
                const string sql = @"
                SELECT U.UserName, L.Wins, L.Losses, L.WinStreak, L.Balance, L.GamesPlayed
                FROM LeaderBoard L
                JOIN Users U ON L.UserId = U.UserID
                ORDER BY L.Balance DESC";
                return await QueryAsync(sql);
            }

            // Get earnings per player for a specific game type.
            public async Task<List<LeaderboardEntry>> GetSpecificGameEarningsAsync(string gameType)
            {
                const string sqlTemplate = @"
                SELECT U.UserName, L.Wins, L.Losses, L.WinStreak, L.Balance, L.GamesPlayed
                FROM LeaderBoard L
                JOIN Users U ON L.UserId = U.UserID
                WHERE L.GameType = @GameType
                ORDER BY L.Balance DESC";
                return await QueryAsync(sqlTemplate, cmd => cmd.Parameters.AddWithValue("@GameType", gameType));
            }

            // Get player rankings sorted by a chosen field (e.g. "Wins" or "Balance").
            public async Task<List<LeaderboardEntry>> GetPlayerRankingsAsync(string sortBy)
            {
                // sortBy: "wins", "losses", "winStreak", "balance", "gamesPlayed"
                string orderColumn = sortBy switch
                {
                    "wins" => "L.Wins DESC",
                    "losses" => "L.Losses DESC",
                    "winStreak" => "L.WinStreak DESC",
                    "balance" => "L.Balance DESC",
                    "gamesPlayed" => "L.GamesPlayed DESC",
                    _ => "L.Wins DESC"
                };
                string sql = $@"
                SELECT U.UserName, L.Wins, L.Losses, L.WinStreak, L.Balance, L.GamesPlayed
                FROM LeaderBoard L
                JOIN Users U ON L.UserId = U.UserID
                ORDER BY {orderColumn}";

                return await QueryAsync(sql);
            }
            private async Task<List<LeaderboardEntry>> QueryAsync(string sql, Action<SqlCommand> configure = null)
            {
                var list = new List<LeaderboardEntry>();
                using var conn = database.GetConnection();
                await conn.OpenAsync();

                using var cmd = new SqlCommand(sql, conn);
                configure?.Invoke(cmd);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new LeaderboardEntry
                    {
                        Username = reader.GetString(0),
                        Wins = reader.GetInt32(1),
                        Losses = reader.GetInt32(2),
                        WinStreak = reader.GetInt32(3),
                        Balance = reader.GetDecimal(4),
                        GamesPlayed = reader.GetInt32(5)
                    });
                }
                return list;
            }
        }
    }

}
