import React, { useEffect, useState } from 'react';
import './Solitaire.css';

const SUITS = ['Hearts', 'Diamonds', 'Clubs', 'Spades'];

function getCardLabel(value) {
  if (value === 1) return 'A';
  if (value === 11) return 'J';
  if (value === 12) return 'Q';
  if (value === 13) return 'K';
  return value.toString();
}

function getCardColor(suit) {
  return suit === 'Hearts' || suit === 'Diamonds' ? 'red' : 'black';
}

function renderCard(card, index, draggable = false, onDragStart = null) {
  const label = getCardLabel(card.number);
  const color = getCardColor(card.suit);
  const suitIcon = `/${card.suit.slice(0, -1)}.png`;

  return (
    <div
      className="card small-card"
      key={index}
      style={{ left: `${index * 15}px` }}
      draggable={draggable}
      onDragStart={e => {
        if (onDragStart) onDragStart(e, card);
      }}
    >
      <div className={`card-content ${color}`}>
        <div className="card-corner top">
          <div>{label}</div>
          <img src={suitIcon} className="corner-icon" />
        </div>
        <div className="card-corner bottom">
          <img src={suitIcon} className="corner-icon" />
          <div>{label}</div>
        </div>
      </div>
    </div>
  );
}

function Solitaire({ username }) {
  const [gameState, setGameState] = useState(null);
  const [draggedCard, setDraggedCard] = useState(null);
  const [fromPile, setFromPile] = useState(null);

  useEffect(() => {
    fetch('/api/solitaire/start', { method: 'POST' })
      .then(() => fetch('/api/solitaire/state'))
      .then(res => res.json())
      .then(data => {
        if (data.success) {
          setGameState(data.state);
        }
      });
  }, []);

  const handleDraw = () => {
    fetch('/api/solitaire/draw', { method: 'POST' })
      .then(() => fetch('/api/solitaire/state'))
      .then(res => res.json())
      .then(data => data.success && setGameState(data.state));
  };

  const handleDrop = (toPile) => {
    if (!draggedCard || !fromPile) return;

    fetch('/api/solitaire/move', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        card: draggedCard,
        fromPile: fromPile,
        toPile: toPile
      })
    })
      .then(res => res.json())
      .then(data => {
        if (data.success) {
          setGameState(data.gameState);
        } else {
          alert(data.message);
        }
        setDraggedCard(null);
        setFromPile(null);
      });
  };

  if (!gameState) return <div className="table-screen" style={{ color: "white" }}>Loading...</div>;

  return (
    <div className="table-screen">
      <div style={{
        position: 'absolute',
        top: '10px',
        left: '10px',
        color: 'white',
        fontSize: '18px'
      }}>{username}</div>

      {/* Stock + Waste */}
      <div style={{ position: 'absolute', top: '60px', left: '60px' }}>
        <button onClick={handleDraw}>Draw</button>
        <div style={{ display: 'flex', marginTop: '8px' }}>
          <div className="card-back small-card" />
          {gameState.waste.length > 0 &&
            renderCard(
              gameState.waste[gameState.waste.length - 1],
              0,
              true,
              (e, card) => {
                setDraggedCard(card);
                setFromPile("Waste");
              }
            )}
        </div>
      </div>

      {/* Foundation */}
      {SUITS.map((suit, i) => {
        const foundation = gameState.foundation[suit] || [];
        return (
          <div
            key={suit}
            style={{ position: 'absolute', top: '60px', left: `${220 + i * 100}px` }}
            onDragOver={(e) => e.preventDefault()}
            onDrop={() => handleDrop(suit)}
          >
            {foundation.length > 0
              ? renderCard(foundation[foundation.length - 1], 0)
              : <div className="card small-card" />}
          </div>
        );
      })}

      {/* Tableau */}
      {gameState.tableau.map((pile, index) => {
        const pileName = "Pile" + (index + 1);
        return (
          <div
            key={index}
            style={{ position: 'absolute', top: '200px', left: `${60 + index * 110}px` }}
            onDragOver={(e) => e.preventDefault()}
            onDrop={() => handleDrop(pileName)}
          >
            {pile.faceDown.map((_, i) => (
              <div key={`fd-${i}`} className="card-back small-card" style={{ top: `${i * 20}px`, position: 'absolute' }} />
            ))}
            {pile.faceUp.map((card, i) => (
              <div
                key={`fu-${i}`}
                style={{ position: 'absolute', top: `${(pile.faceDown.length + i) * 20}px` }}
              >
                {renderCard(card, i, true, (e, c) => {
                  setDraggedCard(c);
                  setFromPile(pileName);
                })}
              </div>
            ))}
          </div>
        );
      })}
    </div>
  );
}

export default Solitaire;
