using System.Collections.Generic;
using CardGamesSolution.Server.Shared;


namespace CardGamesSolution.Server.Solitaire {
    public interface ISolitaireEngine {
        void InitializeGame(List<Card> cards);
        MoveResult ProcessMove(Card card, string fromPile, string toPile);
        bool CheckForWin();
        GameState GetGameState();
    }
    // defines logic operations
}
