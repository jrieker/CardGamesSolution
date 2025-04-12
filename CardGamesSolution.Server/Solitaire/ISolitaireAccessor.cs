using System.Collections.Generic;

namespace CardGamesSolution.Server.Solitaire {
    public interface ISolitaireAccessor {
        List<Card> GetCards();
        Dictionary<string, string> GetCardImages();
        string LoadRules();
        void SaveGame(GameState gameState);
        GameState LoadSavedGame();
    }
    // none of this is really implemented yet
}
