namespace CardGamesSolution.Server.UserAccount
{
    public interface ISessionEngine
    {
        void AddUser(User user);
        void RemoveUser(User user);
        void StartGame();
    }
}
