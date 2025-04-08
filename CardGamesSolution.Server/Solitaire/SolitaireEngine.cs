using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.Solitaire
{
    public class SolitaireEngine : ISolitaireEngine
    {
        private GameState gameState;

        public void InitializeGame(List<Card> cards)
        {
            gameState = new GameState();

            int index = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    gameState.Tableau[i].Add(cards[index++]);
                }
            }

            // deals 1 card to pile 1, 2 to cards pile 2, and so on

            // all leftover cards go to stock pile
            for (; index < cards.Count; index++)
            {
                gameState.Stock.Add(cards[index]);
            }
        }

        public MoveResult ProcessMove(Card card, string fromPile, string toPile)
        {
            // have not implemented this yet, need logic here 
            return new MoveResult(true, "Move accepted.", gameState);
        }

        public bool CheckForWin()
        {
            return gameState.Foundation.Count == 52;
            // game ends when all 52 cards are in the foundation
        }

        public GameState GetGameState()
        {
            return gameState;
            // shows gamestate
        }
    }
}
