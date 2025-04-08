using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Solitaire
{
    public interface ISolitaireAccessor : ICardGamesAccessor
    {
        List<Card> GetCards();
        Dictionary<string, string> GetCardImages(); // You can replace with Image later
        string LoadRules();
        void SaveGame(GameState gameState);
        GameState LoadSavedGame();
    }
}
