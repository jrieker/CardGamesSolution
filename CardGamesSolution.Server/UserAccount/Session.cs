using CardGamesSolution.Server.Solitaire;
using CardGamesSolution.Server.Blackjack;
using CardGamesSolution.Server.Database;

namespace CardGamesSolution.Server.UserAccount
{
    
    public class Session : ISessionEngine
    {
        // List to store active users in the session
        private List<User> users = new List<User>();
        private IUserDataAccessor userDataAccessor;

        public Session(IUserDataAccessor userDataAccessor)
        {
            this.userDataAccessor = userDataAccessor;
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public void RemoveUser(User user)
        {
            users.Remove(user);
        }

        public void StartGame()
        {
            while (true)
            {
            Console.WriteLine("Welcome to the Game Hub!");
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1) Play Solitaire");
            Console.WriteLine("2) Play Blackjack");
            Console.WriteLine("3) Exit");

            Console.Write("Enter your choice (1/2/3): ");
            string input = Console.ReadLine() ?? string.Empty.Trim().ToLower();
            switch (input)
            {
                case "1":
                case "solitaire":
                    Console.WriteLine("Starting Solitaire...\n");
                    ISolitaireEngine engine = new SolitaireEngine();
                    ISolitaireManager manager = new SolitaireManager (engine);
                    manager.StartNewGame();
                    GameState state = engine.GetGameState();
                    SolitaireService game = new SolitaireService();
                    game.StartGame ();
                    break;

                case "2":
                case "blackjack":
                    while (true) 
                    {
                        Console.Write("Would you like to log in more players? (yes/no): ");
                        string response = Console.ReadLine() ?? string.Empty.Trim().ToLower();
                        if (response == "yes")
                        {
                            //users.Add(LoginManager.Login());
                        } 
                        else if (response == "no")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Input");
                        }
                    }
                    Console.WriteLine("Starting Blackjack...\n");
                    break;

                case "3":
                case "exit":
                    Console.WriteLine("Exiting the game. Goodbye!");
                    foreach (User u in users)
                    {
                        userDataAccessor.SaveUserData(u);
                    }
                    return;

                default:
                    Console.WriteLine("Invalid input. Please try again.\n");
                    break;
                }
            }
        }
    }
}
