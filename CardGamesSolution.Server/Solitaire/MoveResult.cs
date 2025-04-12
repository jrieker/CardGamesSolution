namespace CardGamesSolution.Server.Solitaire {
    public class MoveResult {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public GameState GameState { get; set; }

        public MoveResult(bool isValid, string message, GameState gameState) {
            IsValid = isValid;
            Message = message;
            GameState = gameState;
        }
    }
    // represents the result of a move, including if it was valid and updated game state
}
