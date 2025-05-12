using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Blackjack
{
    public interface IPlayer
    {
        int PlayerId { get; set; }
        string PlayerName { get; set; }
        float Balance { get; set; }
        Hand PlayerHand { get; set; }
        float BetValue { get; set; }
        bool HasPlacedBet { get; set; }
        bool HasStood { get; set; }
        bool IsBusted { get; }
        int Outcome { get; set; }

        void UpdateBalance(float amount);
        void PlaceBet(float amount);
    }
}
