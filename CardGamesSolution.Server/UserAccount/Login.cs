namespace CardGamesSolution.Server.UserAccount
{
    public class Login : ILoginManager
    {
        public void Login(string username, string password)
        {
            // TODO: Authenticate user with given credentials
        }

        public void Logout(string username)
        {
            // TODO: Log out the user (e.g., end session, update state)
        }

        public void Register(string username, string password)
        {
            // TODO: Create new user and save to database or memory
        }
    }
}
