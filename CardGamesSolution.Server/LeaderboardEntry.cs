namespace CardGamesSolution.Server.Leaderboard
{
    using System.Text.Json.Serialization;

        /// Represents a single leaderboard entry (scoreboard result) for a player.
  
        public class LeaderboardEntry
        {
            /// Gets or sets the username of the player.
            public required string Username { get; set; }

            /// Gets or sets the total number of wins.
            public int Wins { get; set; }

            /// Gets or sets the total number of losses.
            public int Losses { get; set; }

            /// Gets or sets the current consecutive win streak.
            public int WinStreak { get; set; }

            /// Gets or sets the player's account balance (monetary value).
            public decimal Balance { get; set; }

            /// Gets or sets the total number of games played.
            public int GamesPlayed { get; set; }
        }

}
