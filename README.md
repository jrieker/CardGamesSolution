# CardGamesSolution

How to Start the CardGames Web App:

To run the Solitaire or Blackjack game, first make sure you have Node.js (for the frontend) and .NET 8 SDK (for the backend) installed. Open a terminal and start the backend by going into the CardGamesSolution.Server folder and running dotnet run. Then on a local web browser open the http://localhost:3000

How the Login Functionality Works:

- Before playing users must login through a login screen
- A username and password are required to start the game

Solitaire

- The frontend is built with React and displays the game board, cards, and controls
- The backend is written in C# and handles all game logic, like validating moves, drawing cards, and tracking game state

Cards are organized into four main areas:

 - Stock: where you draw new cards from
 - Waste: holds the most recent drawn card
 - Tableau: the 7 main columns where you stack cards
 - Foundation: where you build up suits from Ace to King to win

How to Play:

 - Clicking a card selects it; clicking a valid spot moves it
 - The game only allows legal moves based on Solitaire rules
 - The UI updates automatically after every move using the game state sent from the backend





Blackjack
