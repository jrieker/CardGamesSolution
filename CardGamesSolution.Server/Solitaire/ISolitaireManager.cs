using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Solitaire
{
    public interface ISolitaireManager : ICardGamesManager
    {
        void StartNewGame();
        void Move(Card card, string fromPile, string toPile);
        void DealCards();  // Optional - if used for animations
        void Restart();
        GameState GetGameState();
    }
}
