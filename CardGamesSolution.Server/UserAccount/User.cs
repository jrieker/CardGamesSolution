using System;
using Microsoft.Data.SqlClient;

namespace CardGamesSolution.Server.UserAccount
{
    public class User : IUserAccessor
    {
        // Properties
        public string Username { get; set; }
        public string Password { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public float Balance { get; set; }

        //Constructors
        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
            this.Wins = 0;
            this.Losses = 0;
            this.Balance = 0f;
        }

        public User(string username, string password, int wins, int losses, float balance)
        {
            this.Username = username;
            this.Password = password;
            this.Wins = wins;
            this.Losses = losses;
            this.Balance = balance;
        }

        // Method placeholders
        public void SaveStats()
        {
            // TODO: Save user stats to database
        }

        public void UpdateBalance(float amount)
        {
            this.Balance += amount;
        }

        public void AddWin()
        {
            this.Wins++;
        }

        public void AddLoss()
        {
            this.Losses++;
        }

        public void TestDatabaseConnection()
        {
            string connString="Data Source=localhost,1433;Initial Catalog=master;User ID=sa;Password=SuperSecure123;TrustServerCertificate=true";
            Console.WriteLine("Getting Connection ...");
            SqlConnection conn = new SqlConnection(connString);
            try
            {
            Console.WriteLine("Openning Connection ...");
            conn.Open();
            Console.WriteLine("Connection successful!");
            }
            catch (Exception e)
            {
            Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}
