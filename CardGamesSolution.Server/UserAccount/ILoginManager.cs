namespace CardGamesSolution.Server.UserAccount
{
    public interface ILoginManager
    {
        void Login(string username, string password);
        void Logout(string username);
        void Register(string username, string password);
    }
}
