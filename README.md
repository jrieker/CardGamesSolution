# CardGamesSolution

How to Start the CardGames Web App:

To run the Solitaire or Blackjack game, first make sure you have Node.js (for the frontend) and .NET 8 SDK (for the backend) installed. Open a terminal and start the backend by going into the CardGamesSolution.Server folder and running dotnet run. Then on a local web browser open the http://localhost:3000

## How the Login Functionality Works:

- Before playing users must login through a login screen
- If the user does not already have an account, one can be created
- Up to 6 users can be logged in at a time while playing Blackjack
- Only one player can be logged in while playing Solitaire
- User stats (balance, wins, etc.) are saved to the database at the end of the round.

## Solitaire

- The frontend is built with React and displays the game board, cards, and controls
- The backend is written in C# and handles all game logic, like validating moves, drawing cards, and tracking game state
- The UI updates automatically after every move using the game state sent from the backend


### Cards are organized into four main areas:

 - Stock: where you draw new cards from
 - Waste: holds the most recent drawn card
 - Tableau: the 7 main columns where you stack cards
 - Foundation: where you build up suits from Ace to King to win

### How to Play:

 - Clicking a card selects it; clicking a valid spot moves it
 - When clicking on a stack to move it, click the highest number card to move the stack
 - The game only allows legal moves based on Solitaire rules
 - Build four foundation piles by suit from Ace to King to win
 - Only alternating colors and descending numbers can stack in the tableau
 - Only a King (or stack starting with a King) can be moved to an empty tableau column
 - Move cards from the waste or tableau to the foundation if they are the next in suit and order
 - Draw cards from the stock pile into the waste, one at a time
 - You can move full stacks between tableau columns if they follow the color and order rule

## Blackjack

- The frontend is built using react and handles the display of the cards, hands, and players
- The backend is built using C#, and handles all game logic from placing a bet to processing an end of round

### Three Key hands

There are three key hands:

 - Your hand
 - Dealers Hand
 - (Optional) Others Players' hands

### How to Play:

1. After the initial deal, you have two cards and review your total  
2. Choose “Hit” to draw another card; choose “Stand” to end your turn  
3. If your hand exceeds 21, you bust and lose immediately  
4. After all players finish, the dealer reveals their face-down card  
5. The dealer hits until their total is 17 or higher  
6. Compare your total to the dealer’s; the hand closest to 21 without busting wins  
7. If the dealer busts, all remaining players win 
