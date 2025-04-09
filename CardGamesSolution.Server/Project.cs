using CardGamesSolution.Server.UserAccount;

class Program {
    static void Main(string[] args) {
        Session session = new Session();
        User primaryUser = LoginManager.Login();
        session.StartGame();
    }
}