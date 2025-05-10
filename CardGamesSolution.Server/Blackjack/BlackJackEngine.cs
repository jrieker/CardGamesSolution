using CardGamesSolution.Server.Database;
using CardGamesSolution.Server.Shared;
using CardGamesSolution.Server.UserAccount;

namespace CardGamesSolution.Server.Blackjack
{
    public class BlackJackEngine : IBlackJackEngine
    {
        private List<Player> _players = new List<Player>();
        private Deck _deck = new Deck();
        private Hand _dealerHand = new Hand();
        private int currentTurnIndex = 0;
        private bool dealerSecondCardFlipped = false;

        private readonly UserDataAccessor _userDataAccessor;

        public BlackJackEngine(UserDataAccessor userDataAccessor)
        {
            _userDataAccessor = userDataAccessor;
        }

        public List<Player> Intialize(User[] users)
        {
            _players = users.Select(u => new Player(u.UserId, u.Username, u.Balance)).ToList();

            Console.WriteLine("Blackjack game started with players:");
            foreach (var player in _players)
            {
                Console.WriteLine($"- {player.PlayerName} (${player.Balance})");
            }

            _deck = new Deck();
            _deck.Shuffle();
            _dealerHand = new Hand();
            currentTurnIndex = 0;
            dealerSecondCardFlipped = false;

            return _players;
        }

        public int GetCurrentTurnIndex() => currentTurnIndex;

        public object DealInitialCards()
        {
            foreach (var player in _players)
            {
                player.PlayerHand = new Hand();
            }

            _deck = new Deck();
            _deck.Shuffle();
            _dealerHand = new Hand();
            dealerSecondCardFlipped = false;

            foreach (var player in _players)
            {
                player.PlayerHand.AddCard(_deck.Draw());
            }

            _dealerHand.AddCard(_deck.Draw());

            foreach (var player in _players)
            {
                player.PlayerHand.AddCard(_deck.Draw());
            }

            _dealerHand.AddCard(_deck.Draw());

            Console.WriteLine("\n--- Hands Dealt ---");
            foreach (var player in _players)
            {
                Console.Write($"{player.PlayerName}: ");
                foreach (var card in player.PlayerHand.getCards())
                    Console.Write($"{card.Number} of {card.Suit}, ");
                Console.WriteLine();
            }

            Console.Write("Dealer: ");
            var dealerCards = _dealerHand.getCards();
            Console.WriteLine($"{dealerCards[0].Number} of {dealerCards[0].Suit}, [face down]");

            currentTurnIndex = 0;

            return new
            {
                players = _players.Select(p => new
                {
                    userId = p.PlayerId,
                    username = p.PlayerName,
                    balance = p.Balance,
                    handValue = p.PlayerHand.valueOfHand(),
                    cards = p.PlayerHand.getCards().Select(c => new { number = c.Number, suit = c.Suit })
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
            var player = _players.FirstOrDefault(p => p.PlayerName == username);
            if (player == null)
                return new { success = false, message = "Player not found" };

            if (amount > player.Balance)
            {
                if (player.Balance == 0 && amount == 5)
                {
                    player.BetValue = amount;
                    Console.WriteLine($"{player.PlayerName} has 0 balance but gets a free $5 bet.");

                    if (currentTurnIndex < _players.Count - 1)
                        currentTurnIndex++;

                    return new { success = true, currentTurnIndex };
                }

                return new { success = false, message = "Cannot bet more than current balance." };
            }

            player.BetValue = amount;
            player.Balance -= amount;

            Console.WriteLine($"{player.PlayerName} bets {amount}. New balance: {player.Balance}");

            if (currentTurnIndex < _players.Count - 1)
                currentTurnIndex++;

            return new { success = true, currentTurnIndex };
        }

        public object Stand(int userId)
        {
            var player = _players.FirstOrDefault(p => p.PlayerId == userId);
            if (player == null) return new { success = false };

            Console.WriteLine($"{player.PlayerName} stands.");

            if (currentTurnIndex < _players.Count - 1)
                currentTurnIndex++;

            return new { success = true, currentTurnIndex };
        }

        public object Double(int userId)
        {
            var player = _players.FirstOrDefault(p => p.PlayerId == userId);
            if (player == null) return new { success = false, message = "Player not found" };

            float currentBet = player.BetValue;

            if (player.Balance < currentBet)
            {
                Console.WriteLine($"{player.PlayerName} tried to double but only has ${player.Balance} left.");
                return new { success = false, message = "Insufficient balance to double." };
            }

            player.BetValue = currentBet * 2;
            player.Balance -= currentBet;

            Console.WriteLine($"{player.PlayerName} doubles to {player.BetValue}. New balance: {player.Balance}");

            return new { success = true, bet = player.BetValue };
        }

        public object Hit(int userId)
        {
            var player = _players.FirstOrDefault(p => p.PlayerId == userId);
            if (player == null) return new { success = false };

            var card = _deck.Draw();
            player.PlayerHand.AddCard(card);
            int handValue = player.PlayerHand.valueOfHand();

            Console.WriteLine($"{player.PlayerName} hits and draws {card.Number} of {card.Suit} (Total: {handValue})");

            bool isBusted = handValue > 21;
            bool isPerfect21 = handValue == 21;

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
            bool flippedSecondCard = false;
            Card? drawnCard = null;
            int handValue;
            bool shouldContinue = true;
            string? winner = null;

            if (!dealerSecondCardFlipped)
            {
                dealerSecondCardFlipped = true;
                flippedSecondCard = true;
                handValue = _dealerHand.valueOfHand();

                var nonBustedPlayers = _players.Where(p => p.PlayerHand.valueOfHand() <= 21).ToList();
                int maxPlayerValue = nonBustedPlayers.Max(p => p.PlayerHand.valueOfHand());

                if (handValue == maxPlayerValue && handValue > 17)
                {
                    var tiedPlayers = nonBustedPlayers.Where(p => p.PlayerHand.valueOfHand() == handValue).ToList();

                    foreach (var p in tiedPlayers)
                    {
                        p.Balance += p.BetValue;
                        Console.WriteLine($"{p.PlayerName} pushes with dealer and gets back ${p.BetValue}. New balance: ${p.Balance}");
                    }

                    winner = string.Join(", ", tiedPlayers.Select(p => p.PlayerName)) + " Push";
                    Console.WriteLine("Dealer and player(s) tied at " + handValue + ". Push: " + winner);
                    shouldContinue = false;
                }
                else
                {
                    bool dealerBeatsAll = nonBustedPlayers.All(p => handValue > p.PlayerHand.valueOfHand());
                    if (handValue <= 21 && dealerBeatsAll)
                    {
                        winner = "Dealer Wins";
                        shouldContinue = false;
                        Console.WriteLine("Dealer wins immediately after revealing second card.");
                    }
                }
            }
            else
            {
                (drawnCard, handValue, shouldContinue) = DealerStepDraw(_dealerHand, _deck, _players);

                if (!shouldContinue)
                {
                    var nonBustedPlayers = _players.Where(p => p.PlayerHand.valueOfHand() <= 21).ToList();
                    int maxPlayerValue = nonBustedPlayers.Max(p => p.PlayerHand.valueOfHand());

                    if (handValue > 21 && drawnCard != null)
                    {
                        int preValue = handValue - CalculateVisibleDealerCard(drawnCard);
                        var winners = _players.Where(p => p.PlayerHand.valueOfHand() <= 21 && p.PlayerHand.valueOfHand() >= preValue).ToList();

                        foreach (var p in winners)
                        {
                            p.Balance += p.BetValue * 2;
                            Console.WriteLine($"{p.PlayerName} wins and now has ${p.Balance}");
                        }

                        winner = string.Join(", ", winners.Select(p => p.PlayerName)) + " Wins";
                        Console.WriteLine($"Dealer busts. Winners: {winner}");
                    }
                    else if (handValue == maxPlayerValue && handValue > 17)
                    {
                        var tiedPlayers = nonBustedPlayers.Where(p => p.PlayerHand.valueOfHand() == handValue).ToList();

                        foreach (var p in tiedPlayers)
                        {
                            p.Balance += p.BetValue;
                            Console.WriteLine($"{p.PlayerName} pushes with dealer and gets back ${p.BetValue}. New balance: ${p.Balance}");
                        }

                        winner = string.Join(", ", tiedPlayers.Select(p => p.PlayerName)) + " Push";
                        Console.WriteLine("Dealer and player(s) tied at " + handValue + ". Push: " + winner);
                    }
                    else
                    {
                        winner = "Dealer Wins";
                        Console.WriteLine("Dealer wins after drawing cards.");
                    }
                }
            }

            return new
            {
                flippedSecondCard,
                drawnCard = drawnCard == null ? null : new { number = drawnCard.Number, suit = drawnCard.Suit },
                handValue,
                cards = _dealerHand.getCards().Select(c => new { number = c.Number, suit = c.Suit }),
                shouldContinue,
                winner
            };
        }

        public (Card? drawnCard, int handValue, bool shouldContinue) DealerStepDraw(Hand dealerHand, Deck deck, List<Player> players)
        {
            int currentDealerValue = dealerHand.valueOfHand();
            var nonBustedPlayers = players.Where(p => p.PlayerHand.valueOfHand() <= 21).ToList();

            if (nonBustedPlayers.Any())
            {
                int maxPlayerValue = nonBustedPlayers.Max(p => p.PlayerHand.valueOfHand());

                if (currentDealerValue == maxPlayerValue && currentDealerValue > 17)
                {
                    Console.WriteLine($"Dealer and player(s) tied at {currentDealerValue}. Push triggered. Dealer stops.");
                    return (null, currentDealerValue, false);
                }
            }

            var card = deck.Draw();
            dealerHand.AddCard(card);
            int dealerValue = dealerHand.valueOfHand();

            Console.WriteLine($"Dealer draws {card.Number} of {card.Suit} (Total: {dealerValue})");

            if (dealerValue >= 21)
                return (card, dealerValue, false);

            bool dealerBeatsEveryone = nonBustedPlayers.All(p => dealerValue > p.PlayerHand.valueOfHand());
            return (card, dealerValue, !dealerBeatsEveryone);
        }

        public object EndRound()
        {

            foreach (var player in _players)
            {
                // ATTEMPTS TO UPDATE DATABASE, THIS DOES NOT WORK
                var user = _userDataAccessor.GetUserByUsername(player.PlayerName);
                user.Balance = player.Balance;

                _userDataAccessor.SaveUserData(user);
                // END ATTEMPTING TO UPDATE DATABASE
                
                player.PlayerHand.clearHand();
                player.BetValue = 0;
            }

            _deck = new Deck();
            _deck.Shuffle();
            _dealerHand = new Hand();
            currentTurnIndex = 0;
            dealerSecondCardFlipped = false;

            Console.WriteLine("New round starting:");
            foreach (var player in _players)
                Console.WriteLine($"- {player.PlayerName} (${player.Balance})");

            return new
            {
                players = _players.Select(p => new
                {
                    userId = p.PlayerId,
                    username = p.PlayerName,
                    balance = p.Balance
                }),
                currentTurnIndex
            };
        }
    }
}
