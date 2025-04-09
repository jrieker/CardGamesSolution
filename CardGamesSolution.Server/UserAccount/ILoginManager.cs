namespace CardGamesSolution.Server.UserAccount
{
    public interface ILoginManager
    {
        User Login(string username, string password);
        void Logout(string username);
        void Register(string username, string password);
    }
}
