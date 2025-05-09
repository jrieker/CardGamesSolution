namespace CardGamesSolution.Server.Leaderboard
{
    namespace CardGamesSolution.Server.Leaderboard
    {
        public interface ILeaderboard
        {
            /// Get total wins for each player across all games.
            Task<List<LeaderboardEntry>> GetAllGameWinsAsync();

            /// Get wins for each player in a specific game type (e.g. "Poker" or "Solitaire")
            Task<List<LeaderboardEntry>> GetSpecificGameWinsAsync(string gameType);

            /// Get total earnings (balance) for each player across all games.
            Task<List<LeaderboardEntry>> GetAllGameEarningsAsync();

            /// Get earnings (balance) for each player in a specific game type.
            Task<List<LeaderboardEntry>> GetSpecificGameEarningsAsync(string gameType);

            /// Get player rankings sorted by a given criterion ("Wins" or "Balance" etc.).

            Task<List<LeaderboardEntry>> GetPlayerRankingsAsync(string sortBy);
        }
    }
}
