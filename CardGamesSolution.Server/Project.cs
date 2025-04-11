using CardGamesSolution.Server.UserAccount;

class Project {
    static void Main(string[] args) {
        Session session = new Session();
        User primaryUser = LoginManager.Login();
        primaryUser.TestDatabaseConnection();
        session.StartGame();
    }
}