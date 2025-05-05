namespace CardGamesSolution.Server.Leaderboard
{
    namespace CardGamesSolution.Server.Leaderboard
    {
        public interface ILeaderboard
        {
            /// <summary>Get total wins for each player across all games.</summary>
            List<LeaderboardEntry> GetAllGameWins();

            /// <summary>Get wins for each player in a specific game type (e.g. "Poker" or "Solitaire").</summary>
            List<LeaderboardEntry> GetSpecificGameWins(string gameType);

            /// <summary>Get total earnings (balance) for each player across all games.</summary>
            List<LeaderboardEntry> GetAllGameEarnings();

            /// <summary>Get earnings (balance) for each player in a specific game type.</summary>
            List<LeaderboardEntry> GetSpecificGameEarnings(string gameType);

            /// <summary>
            /// Get player rankings sorted by a given criterion ("Wins" or "Balance" etc.).
            /// </summary>
            List<LeaderboardEntry> GetPlayerRankings(string sortBy);
        }
    }
}
