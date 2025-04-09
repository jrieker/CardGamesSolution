namespace CardGamesSolution.Server.UserAccount
{
    
    public class LoginManager
    {
        public static User Login()
        {
            Console.WriteLine("=== LOGIN ===");
            Console.Write("Username: ");
            string username = Console.ReadLine().Trim();
            //TODO: Check if username exists in DB
            Console.Write("Password: ");
            string password = Console.ReadLine().Trim();
            //TODO check if password matches
            Console.WriteLine("\nLogging in...\n");
            User user = new User(username, password);
            return user;
        }

        public void Register(string username, string password)
        {
            // TODO: Create new user and save to database
        }
    }
}
