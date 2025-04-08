using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Solitaire
{
    public interface ISolitaireEngine : ICardGamesEngine
    {
        void InitializeGame(List<Card> cards);
        MoveResult ProcessMove(Card card, string fromPile, string toPile);
        bool CheckForWin();
        GameState GetGameState();
    }
}
