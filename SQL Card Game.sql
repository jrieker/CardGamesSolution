

drop table BlackJack;
drop table Solitaire;
drop table LeaderBoard;
drop table Game;
drop table Users;


create table Users(
    UserID int not null,
    userName varchar(255),
    passWord varchar(255),
    Balance int,
	Wins int,
	Losses int,
	gamesPlayed int,
	Primary key (UserID),
);


create table Game(
	GameID int not null,
	playerCount int,
	gameType varchar(255),
	Primary key (GameID),
	UserId int FOREIGN KEY REFERENCES Users(UserID)
);

create table LeaderBoard(
	LeaderboardID int not null,
	Wins int,
	Losses int,
	winStreak int,
	Balance int,
	gamesPlayed int,
	Primary key (LeaderboardID),
	UserId int FOREIGN KEY REFERENCES Users(UserID)
);

create table BlackJack(
	BlackJackID int not null,
	Wins int,
	Losses int,
	winStreak int,
	Balance int,
	gamesPlayed int,
	Primary key (PokerID),
	GameID int FOREIGN KEY REFERENCES Game(GameID),
	LeaderboardID int FOREIGN KEY REFERENCES LeaderBoard(LeaderboardID)
);

create table Solitaire(
	SolitaireID int not null,
	Wins int,
	Losses int,
	winStreak int,
	gamesPlayed int,
	Primary key (SolitaireID),
	GameID int FOREIGN KEY REFERENCES Game(GameID),
	LeaderboardID int FOREIGN KEY REFERENCES LeaderBoard(LeaderboardID)
);


insert into Users (UserID, userName, passWord, balance, wins, losses, gamesPlayed) values (001, 'Elliott', '12345', 100, 0, 0, 0);
insert into Users (UserID, userName, passWord, balance, wins, losses, gamesPlayed) values (002, 'Nick', 'Hello World', 135, 35, 0, 35);
insert into Users (UserID, userName, passWord, balance, wins, losses, gamesPlayed) values (003, 'Garrett', 'Daisy Daisy', 0, 50, 150, 200);
insert into Users (UserID, userName, passWord, balance, wins, losses, gamesPlayed) values (004, 'James', 'abc123', 999, 999, 0, 999);
insert into Users (UserID, userName, passWord, balance, wins, losses, gamesPlayed) values (005, 'Jacob', 'github101', -197, 255, 552, 807);

insert into Game (GameID, gameType, UserId) values (001, 'BlackJack', 002);
insert into Game (GameID, gameType, UserId) values (002, 'BlackJack', 003);
insert into Game (GameID, gameType, UserId) values (003, 'Soltaire', 004);
insert into Game (GameID, gameType, UserId) values (051, 'Soltaire', 003);
insert into Game (GameID, gameType, UserId) values (102, 'BlackJack', 002);
insert into Game (GameID, gameType, UserId) values (343, 'Soltaire', 004);


