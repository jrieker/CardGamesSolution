using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Solitaire {
    public class SolitaireService {
        private readonly ISolitaireManager manager;
        private readonly ISolitaireEngine engine;

        public SolitaireService() {
            engine = new SolitaireEngine();
            manager = new SolitaireManager(engine);
        }

        public void StartGame() {
            manager.StartNewGame();
        }

        public GameState GetState() {
            return engine.GetGameState();
        }

        public MoveResult MoveCard(Card card, string from, string to) {
            return engine.ProcessMove(card, from, to);
        }

        public void DrawFromStock() {
            manager.DrawFromStock();
        }
    }
}
