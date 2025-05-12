namespace CardGamesSolution.Server.UserAccount
{
    public interface IUserDataAccessor
    {
        User GetUserByUsername(string username);
        bool UserExists(string username);
        void SaveUserData(User user);
        int GetNextUserId();
        void DeleteUserByUsername(string username);
    }
}