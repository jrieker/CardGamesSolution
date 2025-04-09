namespace CardGamesSolution.Server.UserAccount
{
    using CardGamesSolution.Server.UserAccount;
    public interface ISessionEngine
    {
        void AddUser(User user);
        void RemoveUser(User user);
        void StartGame();
    }
}
