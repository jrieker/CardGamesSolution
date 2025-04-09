using System.Collections.Generic;

namespace SolitaireEngine {
    public interface ISolitaireManager {
        void StartNewGame();
        void Move(Card card, string fromPile, string toPile);
        void DealCards();  // Placeholder, waiting for UI
        void Restart();
        void DrawFromStock();
        GameState GetGameState();
    }
    // connects engine and UI
}
