namespace CardGamesSolution.Server.Leaderboard.CardGamesSolution.Server.Leaderboard
{
    using System.Text.Json.Serialization;
        /// <summary>
        /// Represents a single leaderboard entry (scoreboard result) for a player.
        /// </summary>
        public class LeaderboardEntry
        {
            /// <summary>Gets or sets the username of the player.</summary>
            [JsonPropertyName("username")]
            public string Username { get; set; }

            /// <summary>Gets or sets the total number of wins.</summary>
            [JsonPropertyName("wins")]
            public int Wins { get; set; }

            /// <summary>Gets or sets the total number of losses.</summary>
            [JsonPropertyName("losses")]
            public int Losses { get; set; }

            /// <summary>Gets or sets the current consecutive win streak.</summary>
            [JsonPropertyName("winStreak")]
            public int WinStreak { get; set; }

            /// <summary>Gets or sets the player's account balance (monetary value).</summary>
            [JsonPropertyName("balance")]
            public decimal Balance { get; set; }

            /// <summary>Gets or sets the total number of games played.</summary>
            [JsonPropertyName("gamesPlayed")]
            public int GamesPlayed { get; set; }
        }

}