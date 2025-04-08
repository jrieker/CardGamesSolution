using CardGamesSolution.Server.Shared;
using System.Numerics;

namespace CardGamesSolution.Server.Blackjack
{
    public class BlackJackManager
    {
        private IBlackJackEngine BlackJackEngine;

        public BlackJackManager(IBlackJackEngine engine)
        {
            BlackJackEngine = engine;
        }

        public void StartGame(Player[] players, Hand dealersHand, Deck deck)
        {
            BlackJackEngine.DealHands(players, dealersHand, deck);
            PlaceBets(players);
            foreach (Player player in players)
            {
                dealersHand.printHand();
                Console.WriteLine($"\n {player.GetPlayerName()}'s Turn --");
                Turn(player, deck);
            }

            BlackJackEngine.PlayDealer(dealersHand, deck);

            Payout(players, dealersHand);
        }

        public void PlaceBets(Player[] players)
        {
            foreach (Player player in players)
            {
                Console.Write($"{player.GetPlayerName()}, enter an amount to wager: ");
                string input = Console.ReadLine();
                if (float.TryParse(input, out float amount) && amount > 0)
                {
                    player.SetBetValue(amount);
                }
                else
                {
                    Console.WriteLine($"{player.GetPlayerName}, Invalid bet. Setting bet to 0.");
                    player.SetBetValue(0);
                }
            }
        }

        public void Payout(Player[] players, Hand dealersHand)
        {
            int dealersHandValue = dealersHand.valueOfHand();


            foreach (Player player in players)
            {
                int playerValue = player.GetPlayerHand().valueOfHand();
                float bet = player.GetBetValue();
                float payout = BlackJackEngine.ComputePayout(playerValue, dealersHandValue, bet);

                string result;

                if (payout > 0)
                {
                    result = $"You won {payout}";
                }
                else
                {
                    payout = Math.Abs(payout);
                    result = $"You lost {payout}";
                }

                Console.WriteLine($"{player.GetPlayerName()} ({playerValue}) {result}");
            }


        }
        public void Turn(Player player, Deck deck)
        {
            var hand = player.GetPlayerHand();
            while (true)
            {

                Console.WriteLine($"{player.GetPlayerName()} hand value: {hand.valueOfHand()} \n");
                player.GetPlayerHand().printHand();

                //Busted
                if (hand.valueOfHand() > 21)
                {
                    Console.WriteLine($"{player.GetPlayerName()} busted");
                    break;
                }

                //Gets Input
                Console.Write($"{player.GetPlayerName()}, Hit or Stay? (H/S): ");
                var input = Console.ReadLine().Trim().ToLower();


                //Evaulates Options
                if (input == "h")
                {
                    int newVal = BlackJackEngine.Hit(hand, deck);
                    Console.WriteLine($"{player.GetPlayerName()} drew. New value: {newVal}");
                }
                else if (input == "s")
                {
                    Console.WriteLine("You stay.");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
        }
    }
