using CardGamesSolution.Server.Shared;
using CardGamesSolution.Server.UserAccount;
using Microsoft.SqlServer.Server;


namespace CardGamesSolution.Server.Blackjack
{
    public class BlackJackManager
    {
        private IBlackJackEngine BlackJackEngine;
        private Player[] players = Array.Empty<Player>();
        private Deck deck = null;
        private Hand dealerHand = null;
        private int currentPlayerIndex;
        private bool isRoundOver;
                            

        public BlackJackManager(IBlackJackEngine engine)
        {
            BlackJackEngine = engine;
        }

        //Returns the updated gamestae of the current game
        public MultiplayerGameState MapToGameState() 
        {
            //Creates empty playerDto list
            PlayerDto[] playersDTOs = new PlayerDto[players.Length];

            //Goes through each player and turns them into a DTO
            for (int i = 0; i < players.Length; i++)
            {
                var player = players[i];

                // Build the HandDto
                var tempCards = new List<CardDto>();
                foreach (var card in player.PlayerHand.getCards())
                    tempCards.Add(new CardDto(card.Suit, card.Number));

                var tempHandDto = new HandDto(
                    tempCards,
                    player.PlayerHand.valueOfHand()
                );

                // Populate the PlayerDto
                playersDTOs[i] = new PlayerDto(
                    player.PlayerId,
                    tempHandDto,
                    player.BetValue,
                    player.HasPlacedBet,
                    player.HasStood,
                    player.IsBusted,
                    player.Outcome
                );
            }



            //Creates a dealers card list and cardDTO
            List<CardDto> dealerCards = new List<CardDto>();
            foreach (var card in dealerHand.getCards())
            {
                dealerCards.Add(new CardDto(card.Suit, card.Number));
            }

            //Creates handDTO
            HandDto dealerHandDto = new HandDto(
                dealerCards,
                dealerHand.valueOfHand()
            );

            //Returns a new mu
            return new MultiplayerGameState
                (
                    playersDTOs,
                    dealerHandDto,
                    currentPlayerIndex,
                    isRoundOver
                );

        }

        //Intializes the game
        public MultiplayerGameState Intialize(User[] users)
        {

            players = new Player[users.Length];
            for (int i = 0; i < users.Length; i++)
            {
                User user = users[i];
                players[i] = new Player(user.UserId, user.Username, user.Balance);
            }

            deck = new Deck();
            isRoundOver = false;
        
            BlackJackEngine.DealHands(players, dealerHand, deck);

            currentPlayerIndex = 0;
            isRoundOver = false;

            return MapToGameState();

        }

        public ActionResultDto PlaceBet(BetCommandDto command)
        {
            //finds the player in the list that matches the players command ID
            Player player = players.FirstOrDefault(p => p.PlayerId == command.PlayerId);

            if (player == null) 
            {
                return new ActionResultDto(false, $"Player {command.PlayerId} not found");
            }

            try
            {
                player.PlaceBet(command.Amount);
                return new ActionResultDto(true, "Bet placed");
            }
            catch (Exception ex)
            {
                return new ActionResultDto(false, ex.Message);
            }

        }

        public ActionResultDto Hit(HitCommandDto cmd)
        {
            Player player = players.FirstOrDefault(p => p.PlayerId == cmd.PlayerId);

            if (player == null)
            {
                return new ActionResultDto(false, "Player not found");
            }
            if (player.PlayerId != players[currentPlayerIndex].PlayerId)
            {
                return new ActionResultDto(false, "Not your turn");
            }

            int total = BlackJackEngine.Hit(player.PlayerHand, deck);

            if (total > 21) {
                player.Outcome = WinnerType.Dealer;
            }

            if (player.IsBusted)
                AdvanceTurn();

            return new ActionResultDto(true, $"Drew Card, new total = {total}");
        }

        public ActionResultDto Stand(StandCommandDto command)
        {
            Player player = players.FirstOrDefault(p => p.PlayerId == command.PlayerId);

            if (player == null)
            {
                return new ActionResultDto(false, "Player not found");
            }

            if (player.PlayerId != players[currentPlayerIndex].PlayerId)
            {
                return new ActionResultDto(false, "Not your turn");
            }
            
            player.HasStood = true;
            AdvanceTurn();
            return new ActionResultDto(true, "Player stood");
        }

        private void AdvanceTurn() 
        {
            bool someoneStillToAct = players.Any(p => !p.HasStood && !p.IsBusted);

            //Updates player loop, uses remainder to keep looping continously
            int next = (currentPlayerIndex + 1) % players.Length;

            if (someoneStillToAct)
            {
                 
                int nextIndex = (currentPlayerIndex + 1) % players.Length;

                //Searches for player who hasn't busted or stood
                while (players[nextIndex].HasStood || players[nextIndex].IsBusted)
                {
                    nextIndex = (nextIndex + 1) % players.Length;
                }
                currentPlayerIndex = nextIndex;
            }
            else
            {
                //Gets dealers total
                int dealerTotal = BlackJackEngine.PlayDealer(dealerHand, deck);   

                //Goes through each player and calculates if they won or not
                foreach (Player player in players)
                {
                    int playerTotal = player.PlayerHand.valueOfHand();
                    float payout = BlackJackEngine.ComputePayout(playerTotal, dealerTotal, player.BetValue);  

                    // Update balance and set Outcome
                    player.UpdateBalance(payout);
                    player.BetValue = 0;

                    if (payout > 0) 
                    {
                        player.Outcome = WinnerType.Player;
                    }
                    else if(payout < 0)
                    {
                        player.Outcome = WinnerType.Dealer;
                    }
                    else
                    {
                        player.Outcome = WinnerType.Push;
                    }

                    

                }

                //end round
                isRoundOver = true;
            }

        }

    }
}
