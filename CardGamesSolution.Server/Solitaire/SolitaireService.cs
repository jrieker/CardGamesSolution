using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Solitaire
{
    public class SolitaireService
    {
        private ISolitaireManager manager = new SolitaireManager(new SolitaireEngine());

        public void StartGame()
        {
            manager = new SolitaireManager(new SolitaireEngine());
            manager.StartNewGame();
        }

        public GameState GetState()
        {
            return manager.GetGameState();
        }

        public void DrawFromStock()
        {
            manager.DrawFromStock();
        }

        public MoveResult MoveCard(Card card, string from, string to)
        {
            return manager.Move(card, from, to);
        }
    }
}
