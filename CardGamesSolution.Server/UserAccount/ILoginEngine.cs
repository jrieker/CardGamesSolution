namespace CardGamesSolution.Server.UserAccount
{
    public interface ILoginEngine
    {
        User Login(string username, string password);
        void Register(string username, string password);
    }
}
