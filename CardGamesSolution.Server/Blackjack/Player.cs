namespace CardGamesSolution.Server.Blackjack
{
    public class Player : IPlayer
    {
        private int PlayerId;
        private string PlayerName;

        private float Balance;

        private Hand PlayerHand;

        private float BetValue = 0;

        public Player(int id, string name, float balance)
        {
            this.PlayerId = id;
            this.PlayerName = name;
            this.Balance = balance;
        }

        //Getters
        public int GetPlayerId()
        {
            return this.PlayerId;
        }

        public string GetPlayerName()
        {
            return this.PlayerName;
        }
        public float GetBalance()
        {
            return this.Balance;
        }

        public Hand GetPlayerHand()
        {
            return this.PlayerHand;
        }

        public float GetBetValue()
        {
            return this.BetValue;
        }

        //Setters
        public void SetPlayerId(int id)
        {
            this.PlayerId = id;
        }
        public void SetPlayerName(string name)
        {
            this.PlayerName = name;
        }
        public void SetBalance(float balance)
        {
            this.SetBalance(balance);
        }

        public void SetPlayerHand(Hand hand)
        {
            this.PlayerHand = hand;
        }

        public void SetBetValue(float betValue)
        {
            this.BetValue = betValue;
        }

        //Methods

        //Upate Balance
        public void UpdateBalance(float amountToBeAdded)
        {
            this.Balance += amountToBeAdded;
        }
    }
}
