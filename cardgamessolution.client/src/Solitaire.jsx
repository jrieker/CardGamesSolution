import React, { useEffect, useState } from 'react';
import './Solitaire.css';

// how suits are displayed


const SUIT_SYMBOLS = {
  Hearts: '♥',
  Diamonds: '♦',
  Clubs: '♣',
  Spades: '♠'
};

// how numbers are displayed

const getCardLabel = (number) => {
  if (number === 1) return 'A';
  if (number === 11) return 'J';
  if (number === 12) return 'Q';
  if (number === 13) return 'K';
  return number.toString();
};

// holds currrent game data and tracks last card clicked

function Solitaire({ username }) {
  const [gameState, setGameState] = useState(null);
  const [selectedCard, setSelectedCard] = useState(null);

// how you start/initialize game, calls start to connect to backend
// gets starting game state

  useEffect(() => {
    const startGame = async () => {
      try {
        await fetch('/api/solitaire/start', { method: 'POST' });
        const res = await fetch('/api/solitaire/state');
        const data = await res.json();
        setGameState(data.state);
      } catch (e) {
        console.error('Failed to initialize game:', e);
      }
    };
    startGame();
  }, []);

// used for when a user moves a card
// tells the specific card, where it is, where you try to move it
// if move is good it updates gamestate and clears selected card
// if move is bad it prints a message for why, but i have this silenced
// right now so that you can play a game without the browser prompting you
// it will just not let you move the attempted move instead of stopping
// and prompting

  const handleMove = async (toPile) => {
    if (!selectedCard) return;

    try {
      const response = await fetch('/api/solitaire/move', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          card: selectedCard.card,
          fromPile: selectedCard.fromPile,
          toPile
        })
      });

      const data = await response.json();
      if (data.success) {
        setGameState(data.gameState);
        setSelectedCard(null);
      } else {
        console.warn('Invalid move:', data.message);
        setSelectedCard(null);
      }
    } catch (e) {
      console.error('Move failed:', e);
      setSelectedCard(null);
    }
  };

// this is used to render every card in tableau waste foundation

  const renderCard = (card, faceUp, index, fromPile) => {
    if (!card) return null;

// ignore null cards for now

    const color = card.suit === 'Hearts' || card.suit === 'Diamonds' ? 'red' : 'black';

// determines card color

    const handleClick = () => {
      if (!faceUp) return;

      if (!selectedCard) {
        setSelectedCard({ card, fromPile });
      } else if (
        selectedCard.card.suit === card.suit &&
        selectedCard.card.number === card.number &&
        selectedCard.fromPile === fromPile
      ) {
        setSelectedCard(null);
      } else {
        handleMove(fromPile);
      }
    };

// Selects if nothing selected, deselects if already selected
// trys the move if different previous card selected

    return faceUp ? (
      <div
      key={`${card.suit}-${card.number}-${fromPile}-${index}`}
        className="card"
        style={{
          top: `${index * 15}px`,
          width: '50px',
          height: '70px',
          position: 'absolute',
          cursor: 'pointer'
        }}
        onClick={handleClick}
      >
        <div className={`card-content ${color}`}>
          <div className="card-corner top">
            {getCardLabel(card.number)}<br />{SUIT_SYMBOLS[card.suit]}
          </div>
          <div className="card-corner bottom">
            {getCardLabel(card.number)}<br />{SUIT_SYMBOLS[card.suit]}
          </div>
        </div>
      </div>
    ) : (
      <div
        key={index}
        className="card-back"
        style={{ top: `${index * 15}px`, position: 'absolute' }}
      />
    );
  };

// stacks cards vertically 15px apart
// position: 'absolute' allows card stacking
// cursor: 'pointer' indicates card can be clicked
// card content has cards visual content

  const renderTableau = () => {
    if (!gameState?.tableau) return null;

// draws the 7 tableau piles
// for each pile face down cards come first

    return (
      <div style={{ display: 'flex', gap: '15px', padding: '20px 40px' }}>
        {gameState.tableau.map((pile, i) => {
          const pileName = `Pile${i + 1}`;
          return (
            <div
              key={i}
              style={{ position: 'relative', width: '50px', height: '400px' }}
              onClick={() => handleMove(pileName)}
            >
              {(pile.faceDown ?? []).map((card, idx) =>
                renderCard(card, false, idx, pileName)
              )}
              {(pile.faceUp ?? []).map((card, idx) =>
                renderCard(card, true, (pile.faceDown?.length ?? 0) + idx, pileName)
              )}
            </div>
          );
        })}
      </div>
    );
  };

// clicks foundation to drop card on, calls handle move with foundation pile as destination

  const handleFoundationClick = (suit) => {
    handleMove(suit);
  };

  if (!gameState) return <div>Loading game...</div>;

  // prevents rendering board before game is actually setup
  // puts username in top left corner of screen
  // has stock and waste layout
  // calls the backend for drawing card and reloads state
  // renders back of card in stock cards avaliable, if not will show empty area and you are out of cards
  // does this for waste after
  // lays out four foundation piles
  // if empty shows dashed box for foundation, if not shows the top card in foundation (doesn't ever do this
  // , this is what I tried to get it to do)

  return (
    <div className="table-screen">
      <div style={{ position: 'absolute', top: '10px', left: '10px', color: 'white' }}>
        {username}
      </div>

      <div style={{ display: 'flex', justifyContent: 'space-between', padding: '20px 40px' }}>
        <div style={{ display: 'flex', gap: '20px' }}>
          <div
            onClick={async () => {
              await fetch('/api/solitaire/draw', { method: 'POST' });
              const res = await fetch('/api/solitaire/state');
              const data = await res.json();
              setGameState(data.state);
              setSelectedCard(null);
            }}
            style={{ cursor: 'pointer' }}
          >
            {gameState.stock?.length ? (
              <div className="card-back" />
            ) : (
              <div style={{ width: '50px', height: '70px', border: '1px solid gray' }} />
            )}
          </div>
          <div>
            {gameState.waste?.length ? (
              renderCard(gameState.waste.at(-1), true, 0, 'Waste')
            ) : (
              <div style={{ width: '50px', height: '70px', border: '1px solid white' }} />
            )}
          </div>
        </div>

        <div style={{ display: 'flex', gap: '20px' }}>
          {['Hearts', 'Diamonds', 'Clubs', 'Spades'].map((suit) => {
            const pile = gameState.foundations?.[suit] ?? [];
            const topCard = pile.at?.(-1);

            return (
              <div
                key={suit}
                onClick={() => handleFoundationClick(suit)}
                style={{
                  width: '50px',
                  height: '70px',
                  border: topCard ? 'none' : '2px dashed white',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  color: 'white',
                  fontSize: '20px',
                  cursor: 'pointer'
                }}
              >
                {topCard
                  ? renderCard(topCard, true, 0, suit)
                  : suit[0]}
              </div>
            );
          })}
        </div>
      </div>

      {renderTableau()}
    </div>
  );
}

export default Solitaire;
