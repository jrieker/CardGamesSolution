using System.Collections.Generic;
using CardGamesSolution.Server.Shared;


namespace CardGamesSolution.Server.Solitaire {
    public class TableauPile {
        public List<Card> FaceDown { get; set; } = new List<Card>();
        public List<Card> FaceUp { get; set; } = new List<Card>();

        // lists to hold cards that are faced down vs faced up
    }
}
