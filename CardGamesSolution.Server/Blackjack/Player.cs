namespace CardGamesSolution.Server.Blackjack
{
    public class Player : IPlayer
    {   
        //Values for player
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public float Balance { get; set; }

        //State variables
        public Hand PlayerHand { get;  set; } = new Hand();
        public float BetValue { get;  set; }
        public bool HasPlacedBet { get;  set; }
        public bool HasStood { get;  set; }
        public bool IsBusted => PlayerHand.valueOfHand() > 21;
        public int Outcome { get; set; } // 0 = loss, 1 = win

        public Player(int id, string name, float balance)
        {
            this.PlayerId = id;
            this.PlayerName = name;
            this.Balance = balance;
        }

        //Methods

        //Upate Balance
        public void UpdateBalance(float amountToBeAdded)
        {
            this.Balance += amountToBeAdded;
        }

        public void PlaceBet(float amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Bet must be positive.", nameof(amount));
            }
            if (amount > Balance)
            {
                throw new InvalidOperationException("Cannot bet more than current balance.");
            }

            BetValue = amount;
            Balance -= amount;
            HasPlacedBet = true;
        }
    }
}
