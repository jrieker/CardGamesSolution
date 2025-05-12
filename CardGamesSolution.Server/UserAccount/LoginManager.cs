namespace CardGamesSolution.Server.UserAccount
{
    public class LoginManager : ILoginManager
    {
        private readonly IUserDataAccessor userDataAccessor;

        public LoginManager(IUserDataAccessor userDataAccessor)
        {
            this.userDataAccessor = userDataAccessor;
        }

        public User Login(string username, string password)
        {
            if (userDataAccessor.UserExists(username))
            {
                var user = userDataAccessor.GetUserByUsername(username);
                if (user.Password == password)
                {
                    return user;
                }
                throw new Exception("Incorrect password!");
            }
            throw new Exception("Username not found!");
        }

        public void Register(string username, string password)
        {
            if (userDataAccessor.UserExists(username))
                throw new Exception("User already exists!");

            int userId = userDataAccessor.GetNextUserId();
            float startingBalance = 500;
            var user = new User(userId, username, password, 0, 0, startingBalance, 0);
            userDataAccessor.SaveUserData(user);
        }
    }

public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

