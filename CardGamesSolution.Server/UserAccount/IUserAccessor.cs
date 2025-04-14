namespace CardGamesSolution.Server.UserAccount
{
    public interface IUserAccessor
    {
        void UpdateBalance(float amount);
        void AddWin();
        void AddLoss();
    }
}
