using CardGamesSolution.Server.UserAccount;
using System.Linq;

namespace CardGamesSolution.Server.Blackjack
{
    public class BlackJackManager : IBlackJackManager
    {
        private readonly BlackJackEngine engine;
        private readonly IUserDataAccessor userDataAccessor;

        public BlackJackManager(BlackJackEngine newEngine, IUserDataAccessor newUserDataAccessor)
        {
            engine = newEngine;
            userDataAccessor = newUserDataAccessor;
        }

        public object Start(User[] users)
        {
            if (users == null || users.Length == 0)
                return new { success = false, message = "At least one user is required to start a game." };

            var initializedPlayers = engine.Intialize(users);

            var result = initializedPlayers.Select(p => new PlayerDto
            {
                UserId = p.PlayerId,
                Username = p.PlayerName,
                Balance = p.Balance
            }).ToList();

            return new
            {
                players = result,
                currentTurnIndex = engine.GetCurrentTurnIndex()
            };
        }

        public object EndRound()
        {
            return engine.EndRound(userDataAccessor);
        }

        public object Deal()
        {
            return engine.DealInitialCards();
        }

        public object Hit(int userId)
        {
            return engine.Hit(userId);
        }

        public object Stand(int userId)
        {
            return engine.Stand(userId);
        }

        public object Double(int userId)
        {
            return engine.Double(userId);
        }

        public object Bet(string username, float amount)
        {
            return engine.RegisterBet(username, amount);
        }

        public object DealerStep()
        {
            return engine.DealerStep();
        }
    }
}
