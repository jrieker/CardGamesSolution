using CardGamesSolution.Server.Shared;
using System.Text.Json;

namespace CardGamesSolution.Server.Solitaire
{
    public class SolitaireAccessor : ISolitaireAccessor
    {
        private const string SaveFilePath = "saved_game.json";

        public List<Card> GetCards()
        {
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            var deck = new List<Card>();

            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    deck.Add(new Card(suit, rank));
                }
            }

            return deck;
        }

        public Dictionary<string, string> GetCardImages()
        {
            // load image file paths, doesnt do anything yet
            return new Dictionary<string, string>();
        }

        public string LoadRules()
        {
            // we can make this read from a file also or just type it out
            return "Standard Solitaire Rules:\nMove cards to foundation from tableau or waste...";
        }

        public void SaveGame(GameState gameState)
        {
            string json = JsonSerializer.Serialize(gameState, new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            });

            // if we need these

            File.WriteAllText(SaveFilePath, json);
        }

        public GameState LoadSavedGame()
        {
            if (!File.Exists(SaveFilePath))
                return null;

            // if we need these

            string json = File.ReadAllText(SaveFilePath);
            return JsonSerializer.Deserialize<GameState>(json);
        }
    }
}
