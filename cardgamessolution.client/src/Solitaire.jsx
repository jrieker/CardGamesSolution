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
  const [selectedCard, setSelectedCard] = useState(null);

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

  const handleMove = async (toPile) => {
    if (!selectedCard) return;

    console.log("Sending move:", selectedCard.card, "from", selectedCard.fromPile, "to", toPile);

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

  const renderCard = (card, faceUp, index, fromPile) => {
    if (!card) return null;
    const color = card.suit === 'Hearts' || card.suit === 'Diamonds' ? 'red' : 'black';

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

  const renderTableau = () => {
    if (!gameState?.tableau) return null;

    return (
      <div style={{ display: 'flex', gap: '15px', padding: '20px 40px' }}>
        {gameState.tableau.map((pile, i) => {
          const pileName = `Pile${i + 1}`;
          const isEmpty = pile.faceUp.length === 0 && pile.faceDown.length === 0;
          return (
            <div
              key={i}
              className={isEmpty ? 'empty-pile' : ''}
              style={{ position: 'relative', width: '50px', height: '400px' }}
              onClick={(e) => {
                if (
                  e.target.classList.contains('card-back') ||
                  e.target.classList.contains('empty-pile')
                ) {
                  handleMove(pileName);
                }
              }}
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

  const handleFoundationClick = (suit) => {
    handleMove(suit);
  };

  if (!gameState) return <div>Loading game...</div>;

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
