namespace CardGamesSolution.Server.UserAccount
{
    public class User : IUserAccessor
    {
        // Properties
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public float Balance { get; set; }

        //Constructors
        public User(int userId, string username, string password)
        {
            this.UserId = userId;
            this.Username = username;
            this.Password = password;
            this.Wins = 0;
            this.Losses = 0;
            this.Balance = 0f;
        }

        public User(int userId, string username, string password, int wins, int losses, float balance)
        {
            this.UserId = userId;
            this.Username = username;
            this.Password = password;
            this.Wins = wins;
            this.Losses = losses;
            this.Balance = balance;
        }

        // Method placeholders
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
