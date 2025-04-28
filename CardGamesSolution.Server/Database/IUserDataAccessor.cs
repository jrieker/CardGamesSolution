using CardGamesSolution.Server.UserAccount;

namespace CardGamesSolution.Server.Database
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