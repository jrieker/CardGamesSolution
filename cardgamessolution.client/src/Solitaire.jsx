import React, { useEffect, useState } from 'react';
import './Solitaire.css';

const SUIT_SYMBOLS = {
  Hearts: '♥',
  Diamonds: '♦',
  Clubs: '♣',
  Spades: '♠'
};

const getCardLabel = (number) => {
  if (number === 1) return 'A';
  if (number === 11) return 'J';
  if (number === 12) return 'Q';
  if (number === 13) return 'K';
  return number.toString();
};

function Solitaire({ username }) {
  const [gameState, setGameState] = useState(null);

  useEffect(() => {
    const startGame = async () => {
      try {
        const startRes = await fetch('/api/solitaire/start', { method: 'POST' });
        if (!startRes.ok) {
          console.error('Failed to start game');
          return;
        }

        const stateRes = await fetch('/api/solitaire/state');
        const stateJson = await stateRes.json();

        setGameState(stateJson.state);
      } catch (err) {
        console.error('Error initializing game:', err);
      }
    };

    startGame();
  }, []);

  const renderCard = (card, faceUp, index) => {
    if (!faceUp) {
      return (
        <div
          key={index}
          className="card-back"
          style={{ top: `${index * 15}px`, position: 'absolute' }}
        ></div>
      );
    }

    const color = card.suit === 'Hearts' || card.suit === 'Diamonds' ? 'red' : 'black';

    return (
      <div
        key={index}
        className="card"
        style={{
          top: `${index * 15}px`,
          width: '50px',
          height: '70px',
          position: 'absolute'
        }}
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
    );
  };

  const renderTableau = () => {
    if (!gameState || !gameState.tableau) return null;

    return (
      <div
        style={{
          display: 'flex',
          gap: '15px',
          padding: '20px 40px',
          alignItems: 'flex-start'
        }}
      >
        {gameState.tableau.map((pile, i) => (
          <div
            key={i}
            style={{
              position: 'relative',
              width: '50px',
              height: '400px'
            }}
          >
            {pile.faceDown.map((card, idx) => renderCard(card, false, idx))}
            {pile.faceUp.map((card, idx) => renderCard(card, true, pile.faceDown.length + idx))}
          </div>
        ))}
      </div>
    );
  };

  return (
    <div className="table-screen">
      <div
        style={{
          position: 'absolute',
          top: '10px',
          left: '10px',
          color: 'white',
          fontSize: '18px'
        }}
      >
        {username}
      </div>

      <div
        style={{
          display: 'flex',
          justifyContent: 'space-between',
          padding: '20px 40px'
        }}
      >
        <div style={{ display: 'flex', gap: '20px' }}>
          <div
            onClick={async () => {
              await fetch('/api/solitaire/draw', { method: 'POST' });
              const res = await fetch('/api/solitaire/state');
              const data = await res.json();
              setGameState(data.state);
            }}
            style={{ cursor: 'pointer' }}
          >
            {gameState?.stock?.length ? (
              <div className="card-back"></div>
            ) : (
              <div
                style={{
                  width: '50px',
                  height: '70px',
                  border: '1px solid gray'
                }}
              />
            )}
          </div>
          <div>
            {gameState?.waste?.length ? (
              renderCard(gameState.waste.at(-1), true, 0)
            ) : (
              <div
                style={{
                  width: '50px',
                  height: '70px',
                  border: '1px solid white'
                }}
              />
            )}
          </div>
        </div>
        <div style={{ display: 'flex', gap: '20px' }}>
          {['Hearts', 'Diamonds', 'Clubs', 'Spades'].map((suit) => {
            const pile = gameState?.foundations?.[suit] || [];
            const topCard = pile.at(-1);

            return topCard ? (
              renderCard(topCard, true, 0)
            ) : (
              <div
                key={suit}
                style={{
                  width: '50px',
                  height: '70px',
                  border: '2px dashed white',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  color: 'white',
                  fontSize: '20px'
                }}
              >
                {suit[0]}
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
