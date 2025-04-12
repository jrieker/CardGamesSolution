namespace CardGamesSolution.Server.Solitaire {
    public class MoveRequest {
        public Card Card { get; set; }
        public string FromPile { get; set; }
        public string ToPile { get; set; }

        public MoveRequest(Card card, string fromPile, string toPile) {
            Card = card;
            FromPile = fromPile;
            ToPile = toPile;
        }
    }
    // represents a players attempt to move a card
}
