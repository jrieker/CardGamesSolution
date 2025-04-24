using CardGamesSolution.Server.Shared;
using CardGamesSolution.Server.UserAccount;

namespace CardGamesSolution.Server.Blackjack
{
    public class BlackJackManager
    {
        private List<Player> _players = new List<Player>();
        private Deck _deck = new Deck();
        private Hand _dealerHand = new Hand();
        private int currentTurnIndex = 0;
        private readonly BlackJackEngine _engine;
        private bool dealerSecondCardFlipped = false;

        public BlackJackManager(BlackJackEngine engine)
        {
            _engine = engine;
        }

        public List<Player> Intialize(User[] users)
        {
            _players = users.Select(u => new Player(u.UserId, u.Username, u.Balance)).ToList();

            Console.WriteLine("Blackjack game started with players:");
            foreach (var player in _players)
            {
                Console.WriteLine($"- {player.GetPlayerName()}");
            }

            _deck = new Deck();
            _deck.Shuffle();
            _dealerHand = new Hand();
            currentTurnIndex = 0;
            dealerSecondCardFlipped = false;

            return _players;
        }

        public int GetCurrentTurnIndex()
        {
            return currentTurnIndex;
        }

        public object DealInitialCards()
        {
            foreach (var player in _players)
            {
                player.SetPlayerHand(new Hand());
            }

            _deck = new Deck();
            _deck.Shuffle();
            _dealerHand = new Hand();
            dealerSecondCardFlipped = false;

            _engine.DealHands(_players, _dealerHand, _deck);

            currentTurnIndex = 0;

            return new
            {
                players = _players.Select(p => new
                {
                    userId = p.GetPlayerId(),
                    username = p.GetPlayerName(),
                    handValue = p.GetPlayerHand().valueOfHand(),
                    cards = p.GetPlayerHand().getCards().Select(c => new { number = c.Number, suit = c.Suit })
                }),
                dealer = new
                {
                    handValue = _dealerHand.getCards().Count > 0 ? CalculateVisibleDealerCard(_dealerHand.getCards()[0]) : 0,
                    cards = _dealerHand.getCards().Select((c, i) => new
                    {
                        number = i == 1 ? 0 : c.Number,
                        suit = i == 1 ? "face-down" : c.Suit
                    }).ToList()
                }
            };
        }

        private int CalculateVisibleDealerCard(Card card)
        {
            if (card.Number == 1) return 11;
            if (card.Number >= 11 && card.Number <= 13) return 10;
            return card.Number;
        }

        public object RegisterBet(string username, float amount)
        {
            var player = _players.FirstOrDefault(p => p.GetPlayerName() == username);
            if (player == null)
                return new { success = false, message = "Player not found" };

            player.SetBetValue(amount);
            Console.WriteLine($"{player.GetPlayerName()} bets {amount}");

            if (currentTurnIndex < _players.Count - 1)
                currentTurnIndex++;

            return new { success = true, currentTurnIndex };
        }

        public object Stand(int userId)
        {
            var player = _players.FirstOrDefault(p => p.GetPlayerId() == userId);
            if (player == null) return new { success = false };

            Console.WriteLine($"{player.GetPlayerName()} stands.");

            if (currentTurnIndex < _players.Count - 1)
                currentTurnIndex++;

            return new { success = true, currentTurnIndex };
        }

        public object Double(int userId)
        {
            var player = _players.FirstOrDefault(p => p.GetPlayerId() == userId);
            if (player == null) return new { success = false };

            float currentBet = player.GetBetValue();
            player.SetBetValue(currentBet * 2);

            Console.WriteLine($"{player.GetPlayerName()} doubles to {player.GetBetValue()}");

            return new { success = true, bet = player.GetBetValue() };
        }

        public object Hit(int userId)
        {
            var player = _players.FirstOrDefault(p => p.GetPlayerId() == userId);
            if (player == null) return new { success = false };

            var hand = player.GetPlayerHand();
            var card = _deck.Draw();
            hand.AddCard(card);
            int handValue = hand.valueOfHand();

            Console.WriteLine($"{player.GetPlayerName()} hits and draws {card.Number} of {card.Suit} (Total: {handValue})");

            bool isBusted = handValue > 21;
            bool isPerfect21 = handValue == 21;

            if (isBusted)
            {
                Console.WriteLine($"{player.GetPlayerName()} busted. Moving to next player.");
            }
            else if (isPerfect21)
            {
                Console.WriteLine($"{player.GetPlayerName()} hit exactly 21. Moving to next player.");
            }

            if ((isBusted || isPerfect21) && currentTurnIndex < _players.Count - 1)
            {
                currentTurnIndex++;
            }

            return new
            {
                success = true,
                newCard = new { number = card.Number, suit = card.Suit },
                handValue,
                isBusted,
                isPerfect21,
                currentTurnIndex
            };
        }

        public object DealerStep()
        {
            if (!dealerSecondCardFlipped)
            {
                dealerSecondCardFlipped = true;
                return new
                {
                    flippedSecondCard = true,
                    drawnCard = (object?)null,
                    handValue = _dealerHand.valueOfHand(),
                    cards = _dealerHand.getCards().Select(c => new { number = c.Number, suit = c.Suit }),
                    shouldContinue = true
                };
            }

            var (drawnCard, handValue, shouldContinue) = _engine.DealerStepDraw(_dealerHand, _deck, _players);

            return new
            {
                flippedSecondCard = false,
                drawnCard = drawnCard == null ? null : new { number = drawnCard.Number, suit = drawnCard.Suit },
                handValue,
                cards = _dealerHand.getCards().Select(c => new { number = c.Number, suit = c.Suit }),
                shouldContinue
            };
        }
    }
}
