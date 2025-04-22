using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Solitaire
{
    public interface ISolitaireManager
    {
        void StartNewGame();
        GameState GetGameState();
        void DrawFromStock();
        MoveResult Move(Card card, string fromPile, string toPile);
    }
    
}
