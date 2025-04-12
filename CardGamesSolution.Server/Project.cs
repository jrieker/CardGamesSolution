using CardGamesSolution.Server.UserAccount;

class Project {
    static void Main(string[] args) {
        Session session = new Session();
        bool loggedIn = false;
        User primaryUser = null;
        while (!loggedIn)
        {
            Console.WriteLine("=== LOGIN ===");
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? string.Empty.Trim();
            Console.Write("Password: ");
            string password = Console.ReadLine() ?? string.Empty.Trim();
            try
            {
                primaryUser = LoginManager.Login(username, password);
                loggedIn = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Login failed: " + e.Message);
            }
        }
        session.AddUser(primaryUser);
        session.StartGame();
    }
}