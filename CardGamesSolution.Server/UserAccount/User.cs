namespace CardGamesSolution.Server.UserAccount
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public float Balance { get; set; }
        public int GamesPlayed { get; set; }

        public User() { }

        public User(int userId, string username, string password)
        {
            this.UserId = userId;
            this.Username = username;
            this.Password = password;
            this.Wins = 0;
            this.Losses = 0;
            this.Balance = 0f;
            this.GamesPlayed = 0;
        }

        public User(int userId, string username, string password, int wins, int losses, float balance, int gamesPlayed)
        {
            this.UserId = userId;
            this.Username = username;
            this.Password = password;
            this.Wins = wins;
            this.Losses = losses;
            this.Balance = balance;
            this.GamesPlayed = gamesPlayed;
        }

        public void UpdateBalance(float amount)
        {
            this.Balance += amount;
        }

        public void AddWin()
        {
            this.Wins++;
        }

        public void AddLoss()
        {
            this.Losses++;
        }
    }
}
