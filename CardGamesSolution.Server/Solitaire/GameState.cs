using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Solitaire
{
    public class GameState
    {
        public List<List<Card>> Tableau { get; set; }
        public List<Card> Foundation { get; set; }
        public List<Card> Stock { get; set; }
        public List<Card> Waste { get; set; }
        public int Score { get; set; }

        public GameState()
        {
            Tableau = new List<List<Card>>();
            for (int i = 0; i < 7; i++) Tableau.Add(new List<Card>());

            Foundation = new List<Card>();
            Stock = new List<Card>();
            Waste = new List<Card>();
            Score = 0;
        }
    }
}
