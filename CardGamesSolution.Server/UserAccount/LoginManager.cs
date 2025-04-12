namespace CardGamesSolution.Server.UserAccount
{
    
    public class LoginManager
    {
        public static User Login(string username, string password)
        {
            Console.WriteLine("\nLogging in...\n");
            User user = DatabaseConnection.GetUserFromUsername(username);

            if (user.Password == password) 
            {
                Console.WriteLine("Logged in as " + username + "\n");
                return user;
            }
            else
            {
                throw new Exception("Incorrect password!");
            }  
        }

        public void Register(string username, string password)
        {
            // TODO: Create new user and save to database
        }
    }
}
