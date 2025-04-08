using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CardGamesSolution.Server.Shared
{
    public class User : IUserAccessor
    {
        // Properties
        public string Username { get; set; }
        public string Password { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public float Balance { get; set; }

        // Method placeholders
        public void SaveStats()
        {
            // TODO: Save user stats to database
        }

        public void UpdateBalance(float amount)
        {
            // TODO: Add amount to balance or subtract if negative
        }

        public void AddWin()
        {
            // TODO: Increment wins
        }

        public void AddLoss()
        {
            // TODO: Increment losses
        }
    }
}
