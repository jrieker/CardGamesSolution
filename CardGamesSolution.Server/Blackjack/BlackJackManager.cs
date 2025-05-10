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
                Console.WriteLine($"- {player.PlayerName} (${player.Balance})");
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
                player.PlayerHand = new Hand();
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

            var hand = player.PlayerHand;
            var card = _deck.Draw();
            hand.AddCard(card);
            int handValue = hand.valueOfHand();

            Console.WriteLine($"{player.PlayerName} hits and draws {card.Number} of {card.Suit} (Total: {handValue})");

            bool isBusted = handValue > 21;
            bool isPerfect21 = handValue == 21;

            if (isBusted)
            {
                Console.WriteLine($"{player.PlayerName} busted. Moving to next player.");
            }
            else if (isPerfect21)
            {
                Console.WriteLine($"{player.PlayerName} hit exactly 21. Moving to next player.");
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

                var nonBustedPlayers = _players
                    .Where(p => p.PlayerHand.valueOfHand() <= 21)
                    .ToList();

                if (!nonBustedPlayers.Any())
                {
                    winner = "Dealer Wins";
                    Console.WriteLine("All players busted. Dealer wins by default.");
                    shouldContinue = false;
                }
                else
                {
                    int maxPlayerValue = nonBustedPlayers
                        .Max(p => p.PlayerHand.valueOfHand());

                    if (handValue == maxPlayerValue && handValue > 17)
                    {
                        var tiedPlayers = nonBustedPlayers
                            .Where(p => p.PlayerHand.valueOfHand() == handValue)
                            .ToList();

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
                        bool dealerBeatsAll = nonBustedPlayers
                            .All(p => handValue > p.PlayerHand.valueOfHand());

                        if (handValue <= 21 && dealerBeatsAll)
                        {
                            winner = "Dealer Wins";
                            shouldContinue = false;
                            Console.WriteLine("Dealer wins immediately after revealing second card.");
                        }
                    }
                }
            }

            else
            {
                (drawnCard, handValue, shouldContinue) = _engine.DealerStepDraw(_dealerHand, _deck, _players);

                if (!shouldContinue)
                {
                    if (handValue > 21 && drawnCard != null)
                    {
                        int drawnCardValue = CalculateVisibleDealerCard(drawnCard);
                        int preValue = handValue - drawnCardValue;

                        var winningPlayers = _players
                            .Where(p => p.PlayerHand.valueOfHand() <= 21 &&
                                        p.PlayerHand.valueOfHand() >= preValue)
                            .ToList();

                        if (winningPlayers.Any())
                        {
                            winner = string.Join(", ", winningPlayers.Select(p => p.PlayerName)) + " Wins";
                            foreach (var p in winningPlayers)
                            {
                                p.Balance += p.BetValue * 2;
                                Console.WriteLine($"{p.PlayerName} wins and now has ${p.Balance}");
                            }
                            Console.WriteLine($"Dealer busts. Winners: {winner}");
                        }
                    }
                    else
                    {
                        bool isHigherOrEqualToAll = _players
                            .Where(p => p.PlayerHand.valueOfHand() <= 21)
                            .All(p => handValue >= p.PlayerHand.valueOfHand());

                        if (handValue <= 21)
                        {
                            var nonBustedPlayers = _players
                                .Where(p => p.PlayerHand.valueOfHand() <= 21)
                                .ToList();

                            if (!nonBustedPlayers.Any())
                            {
                                winner = "Dealer Wins";
                                Console.WriteLine("All players busted. Dealer wins by default.");
                            }
                            else
                            {
                                int maxPlayerValue = nonBustedPlayers
                                    .Max(p => p.PlayerHand.valueOfHand());

                                if (handValue == maxPlayerValue && handValue > 17)
                                {
                                    var tiedPlayers = nonBustedPlayers
                                        .Where(p => p.PlayerHand.valueOfHand() == handValue)
                                        .ToList();

                                    foreach (var p in tiedPlayers)
                                    {
                                        p.Balance += p.BetValue;
                                        Console.WriteLine($"{p.PlayerName} pushes with dealer and gets back ${p.BetValue}. New balance: ${p.Balance}");
                                    }

                                    if (tiedPlayers.Any())
                                    {
                                        winner = string.Join(", ", tiedPlayers.Select(p => p.PlayerName)) + " Push";
                                        Console.WriteLine("Dealer and player(s) tied at " + handValue + ". Push: " + winner);
                                    }
                                    else
                                    {
                                        winner = "Dealer Wins";
                                        Console.WriteLine("Dealer wins after drawing cards.");
                                    }
                                }
                                else
                                {
                                    winner = "Dealer Wins";
                                    Console.WriteLine("Dealer wins after drawing cards.");
                                }
                            }
                        }
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




        public object EndRound()
        {

            foreach (var player in _players)
            {

                // Figure out how to save balances to database

                // user = _userDataAccessor.GetUserByUsername(player.PlayerName)
                // user.Balance = player.Balance
                // _userDataAccesor.SaveUserData(user)

                player.PlayerHand.clearHand();
                player.BetValue = 0;
            }

            _deck = new Deck();
            _deck.Shuffle();
            _dealerHand = new Hand();
            currentTurnIndex = 0;
            dealerSecondCardFlipped = false;

            Console.WriteLine("New round starting: ");
            foreach (var player in _players)
            {
                Console.WriteLine($"- {player.PlayerName} (${player.Balance})");
            }

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
