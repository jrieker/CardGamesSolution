namespace CardGamesSolution.Server.UserAccount
{
    public interface IUserAccessor
    {
        User GetUser(string username);
        void SaveStats(User user);
        void UpdateBalance(User user, float amount);
        void AddWin(User user);
        void AddLoss(User user);
    }
}
