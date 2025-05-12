namespace CardGamesSolution.Server.UserAccount
{
    public interface ILoginManager
    {
        User Login(string username, string password);
        void Register(string username, string password);
    }
}
