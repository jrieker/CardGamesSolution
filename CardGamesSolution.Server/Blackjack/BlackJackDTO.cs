namespace CardGamesSolution.Server.Blackjack
{
    
    public record CardDto(string Suit, int Rank);
    public record HandDto(List<CardDto> Cards, int Total);


    //Action(s) DTOs
    public record BetCommandDto(int PlayerId, int Amount);
    public record HitCommandDto(int PlayerId);
    public record StandCommandDto(int PlayerId);
    public record ActionResultDto(bool Success, string? Message);

    public enum WinnerType { Player, Dealer, Push }

    //public record PlayerDto(
    //    int PlayerId,        
    //    HandDto Hand,        
    //    float Bet,
    //    bool HasPlacedBet,
    //    bool HasStood,       
    //    bool IsBusted,
    //    WinnerType? Winner
    //);
    public record MultiplayerGameState(
        PlayerDto[] Players,
        HandDto DealerHand,
        int CurrentPlayerIndex,
        bool IsRoundOver 
    );

}
