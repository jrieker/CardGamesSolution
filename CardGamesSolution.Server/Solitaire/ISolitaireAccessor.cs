using System.Collections.Generic;
using CardGamesSolution.Server.Shared;


namespace CardGamesSolution.Server.Solitaire {
    public interface ISolitaireAccessor {
        List<Card> GetCards();
        Dictionary<string, string> GetCardImages();
        string LoadRules();
        void SaveGame(GameState gameState);
        GameState LoadSavedGame();
    }
    
}
