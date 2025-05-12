namespace CardGamesSolution.Server.UserAccount
{
    public class LoginManager : ILoginManager
    {
        private readonly LoginEngine _loginEngine;

        public LoginManager(LoginEngine loginEngine)
        {
            _loginEngine = loginEngine;
        }

        public User Login(string username, string password)
        {
            return _loginEngine.Login(username, password);
        }

        public void Register(string username, string password)
        {
            _loginEngine.Register(username, password);
        }
    }

    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
