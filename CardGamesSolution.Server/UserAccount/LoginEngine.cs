namespace CardGamesSolution.Server.UserAccount
{
    public class LoginEngine : ILoginEngine
    {
        private readonly IUserDataAccessor userDataAccessor;

        public LoginEngine(IUserDataAccessor userDataAccessor)
        {
            this.userDataAccessor = userDataAccessor;
        }

        public User Login(string username, string password)
        {
            if (userDataAccessor.UserExists(username))
            {
                User user = userDataAccessor.GetUserByUsername(username);

                if (user.Password == password)
                {
                    return user;
                }
                else
                {
                    throw new Exception("Incorrect password!");
                }
            }
            else
            {
                throw new Exception("Username not found!");
            }
        }

        public void Register(string username, string password)
        {
            if (!userDataAccessor.UserExists(username))
            {
                int userId = userDataAccessor.GetNextUserId();
                float startingBalance = 500;
                User u = new User(userId, username, password, 0, 0, startingBalance, 0);
                userDataAccessor.SaveUserData(u);
            }
            else
            {
                throw new Exception("User already exists!");
            }
        }

    }
}
