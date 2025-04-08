using CardGamesSolution.Server.Shared;

namespace CardGamesSolution.Server.UserAccount
{
    public class Session : ISessionEngine
    {
        // List to store active users in the session
        private List<User> Users = new List<User>();

        public void AddUser(User user)
        {
            // TODO: Add user to the Users list
        }

        public void RemoveUser(User user)
        {
            // TODO: Remove user from the Users list
        }

        public void StartGame()
        {
            // TODO: Start the game logic for the current session
        }
    }
}
