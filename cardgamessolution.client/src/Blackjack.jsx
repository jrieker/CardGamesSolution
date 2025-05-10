import React, { useEffect, useState } from 'react';
import './Blackjack.css';

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

function renderCard(card, index) {
    const label = getCardLabel(card.value);
    const color = getCardColor(card.suit);
    const suitIcon = `/${card.suit.slice(0, -1)}.png`;

    return (
        <div
            className="card stacked-card small-card"
            key={index}
            style={{ left: `${index * 15}px` }}
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

function Blackjack({ loggedInPlayers }) {
    const [players, setPlayers] = useState([]);
    const [activePlayerIndex, setActivePlayerIndex] = useState(null);
    const [bets, setBets] = useState({});
    const [playerHands, setPlayerHands] = useState([]);
    const [playerCounts, setPlayerCounts] = useState([]);
    const [dealerHand, setDealerHand] = useState([]);
    const [dealerCount, setDealerCount] = useState(0);
    const [revealedCards, setRevealedCards] = useState({});
    const [dealerCardsRevealed, setDealerCardsRevealed] = useState(0);
    const [dealStarted, setDealStarted] = useState(false);
    const [dealFinished, setDealFinished] = useState(false);
    const [bustMessageVisible, setBustMessageVisible] = useState(false);
    const [bustMessageText, setBustMessageText] = useState("Bust");
    const [canQuit, setCanQuit] = useState(false);




    useEffect(() => {
        if (!loggedInPlayers || loggedInPlayers.length === 0) return;

        fetch('/api/blackjack/start', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(loggedInPlayers)
        })
            .then(res => res.json())
            .then(data => {
                setPlayers(data.players);
                setActivePlayerIndex(data.currentTurnIndex);
                console.log("Backend players response:", data.players);

            })
            .catch(err => console.error("Error starting game:", err));
    }, [loggedInPlayers]);

    const sleep = (ms) => new Promise(resolve => setTimeout(resolve, ms));

    const revealCardsInSequence = async (dealData) => {
        const totalPlayers = dealData.players.length;
        let reveals = {};

        for (let i = 0; i < totalPlayers; i++) {
            reveals[i] = 1;
            setRevealedCards({ ...reveals });
            await sleep(500);
        }

        setDealerCardsRevealed(1);
        await sleep(500);

        for (let i = 0; i < totalPlayers; i++) {
            reveals[i] = 2;
            setRevealedCards({ ...reveals });
            await sleep(500);
        }

        setDealerCardsRevealed(2);
        await sleep(500);

        setDealStarted(true);
        setDealFinished(true);
        setActivePlayerIndex(0);
    };

    const handleStand = async () => {
        const userId = players[activePlayerIndex].userId;
        const response = await fetch('/api/blackjack/stand', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userId)
        });
        const result = await response.json();
        if (result.success) {
            if (activePlayerIndex === players.length - 1) {
                setActivePlayerIndex(null);
                await sleep(500);
                await startDealerTurn();
            } else {
                setActivePlayerIndex(result.currentTurnIndex);
            }
        }
    };

    const startDealerTurn = async () => {
        let continueDrawing = true;

        while (continueDrawing) {
            const res = await fetch('/api/blackjack/dealer/step', {
                method: 'POST'
            });
            const data = await res.json();

            if (data.flippedSecondCard) {
                setDealerCardsRevealed(2);
            } else {
                setDealerCardsRevealed(prev => prev + 1);
            }

            setDealerHand(data.cards);
            setDealerCount(data.handValue);

            if (data.winner) {
                setBustMessageText(data.winner);
                setBustMessageVisible(true);
                setTimeout(async () => {
                    setBustMessageVisible(false);
                    setBustMessageText("Bust");
                    await handleEndRound(); 
                    setCanQuit(true);
                }, 2000);
            }



            if (!data.shouldContinue) {
                continueDrawing = false;
                break;
            }

            await sleep(500);
        }
    };


    const handleHit = async () => {
        const userId = players[activePlayerIndex].userId;
        const response = await fetch('/api/blackjack/hit', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userId)
        });
        const result = await response.json();
        if (result.success) {
            setPlayerHands(prev => {
                const updated = [...prev];
                updated[activePlayerIndex] = [...(updated[activePlayerIndex] || []), result.newCard];
                return updated;
            });

            setPlayerCounts(prev => {
                const updated = [...prev];
                updated[activePlayerIndex] = result.handValue;
                return updated;
            });

            setRevealedCards(prev => {
                const newLength = (playerHands[activePlayerIndex]?.length || 2) + 1;
                return {
                    ...prev,
                    [activePlayerIndex]: newLength
                };
            });

            if (result.isBusted) {
                setDealFinished(false);
                setBustMessageVisible(true);

                setTimeout(() => {
                    setBustMessageVisible(false);
                    if (activePlayerIndex === players.length - 1) {
                        setActivePlayerIndex(null);
                        startDealerTurn();
                    } else {
                        setActivePlayerIndex(result.currentTurnIndex);
                    }
                    setDealFinished(true);
                }, 1500);
            } else if (result.isPerfect21) {
                setActivePlayerIndex(result.currentTurnIndex);
            }
        }
    };

    const handleDouble = async () => {
        const userId = players[activePlayerIndex].userId;
        const response = await fetch('/api/blackjack/double', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userId)
        });
        const result = await response.json();
        if (result.success) {
            setBets(prev => ({
                ...prev,
                [activePlayerIndex]: {
                    ...prev[activePlayerIndex],
                    amount: result.bet
                }
            }));
        }
    };

    const getChipClass = (amount) => {
        switch (amount) {
            case 5: return 'white-chip';
            case 10: return 'red-chip';
            case 25: return 'green-chip';
            case 50: return 'blue-chip';
            case 100: return 'black-chip';
            default: return '';
        }
    };

    const handleBet = async (playerIndex) => {
        const username = players[playerIndex]?.username;
        const amount = bets[playerIndex]?.amount || 0;

        const response = await fetch('/api/blackjack/bet', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, amount })
        });

        const result = await response.json();
        if (!result.success) return;

        if (playerIndex === players.length - 1) {
            setDealStarted(true);
            setActivePlayerIndex(null);

            const dealRes = await fetch('/api/blackjack/deal', { method: 'POST' });
            const dealData = await dealRes.json();

            setPlayerHands(dealData.players.map(p => p.cards));
            setDealerHand(dealData.dealer.cards);
            setPlayerCounts(dealData.players.map(p => p.handValue));
            setDealerCount(dealData.dealer.handValue);

            await revealCardsInSequence(dealData);
        } else {
            setActivePlayerIndex(result.currentTurnIndex);
        }
    };

    const handleEndRound = async () => {
        const res = await fetch('/api/blackjack/end', {
            method: 'POST'
        });
        const data = await res.json();

        setPlayers(data.players);
        setActivePlayerIndex(data.currentTurnIndex);
        setPlayerHands([]);
        setPlayerCounts([]);
        setDealerHand([]);
        setDealerCount(0);
        setRevealedCards({});
        setDealerCardsRevealed(0);
        setBets({});
        setDealStarted(false);
        setDealFinished(false);
        setBustMessageVisible(false);
        setBustMessageText("Bust");
    };


    return (
        <div className="table-screen">
            {bustMessageVisible && (
                <div style={{
                    position: 'absolute',
                    top: '32%',
                    left: '50%',
                    transform: 'translate(-50%, -50%)',
                    color: 'white',
                    fontSize: '48px',
                    fontWeight: 'bold',
                    textShadow: '0 0 10px black',
                    zIndex: 99
                }}>
                    {bustMessageText}
                </div>
            )}

            {canQuit && !dealStarted && (
                <button className="quit-button" onClick={() => {
                    setPlayers([]);
                    setActivePlayerIndex(null);
                    setPlayerHands([]);
                    setPlayerCounts([]);
                    setDealerHand([]);
                    setDealerCount(0);
                    setRevealedCards({});
                    setDealerCardsRevealed(0);
                    setBets({});
                    setDealStarted(false);
                    setDealFinished(false);
                    setBustMessageVisible(false);
                    setBustMessageText("Bust");
                    setCanQuit(false);
                    window.location.reload();
                }}>
                    Quit to Main Menu
                </button>
            )}

            <div className="dealer-area">
                <div className="player-inner">
                    <div className="card-container">
                        {dealerHand.slice(0, dealerCardsRevealed).map((card, index) =>
                            card.suit === "face-down"
                                ? <div className="card small-card card-back" key={index} style={{ left: `${index * 15}px` }}></div>
                                : renderCard({ value: card.number, suit: card.suit }, index)
                        )}
                    </div>
                    <div className={`count ${dealerCardsRevealed >= 2 ? '' : 'invisible'}`}>{dealerCount}</div>
                </div>
            </div>

            {players.map((player, i) => {
                if (!player) return null;

                const total = players.length;
                const gap = 17;
                let left;

                if (total === 1) left = 50;
                else if (total === 2) left = 50 + (i - 0.5) * gap;
                else if (total === 3) left = 50 + (i - 1) * gap;
                else if (total === 4) left = 50 + (i - 1.5) * gap;
                else if (total === 5) left = 50 + (i - 2) * gap;
                else if (total === 6) left = 8 + i * ((100 - 16) / 5);

                return (
                    <div className="player-area" key={i} style={{ left: `${left}%`, transform: 'translateX(-50%)' }}>
                        <div className="player-inner">
                            <div className="card-container">
                                {(playerHands[i] || []).slice(0, revealedCards[i] || 0).map((card, index) =>
                                    renderCard({ value: card.number, suit: card.suit }, index)
                                )}
                            </div>
                            <div className={`count ${(revealedCards[i] || 0) >= 2 ? '' : 'invisible'}`}>
                                {playerCounts[i]}
                            </div>

                            <div className={`placed-chip ${bets[i]?.visible ? getChipClass(bets[i]?.chip) : 'hidden'}`}>
                                {bets[i]?.chip || '?'}
                            </div>
                            <div className={`amountbet ${bets[i]?.visible ? '' : 'hidden'}`}>
                                ${bets[i]?.amount || 0}
                            </div>

                            <div className={`player-username ${i === activePlayerIndex ? 'active-player' : ''}`}>
                                {player.username}
                            </div>

                            {i === activePlayerIndex && !dealStarted && bets[i]?.visible && (
                                <div className="bet-buttons">
                                    <button className="clear-button" onClick={() => {
                                        setBets(prev => ({
                                            ...prev,
                                            [i]: { chip: null, amount: 0, visible: false }
                                        }));
                                    }}>Clear</button>
                                    <button className="confirm-button" onClick={() => handleBet(i)}>
                                        {i === players.length - 1 ? 'Deal' : 'Bet'}
                                    </button>
                                </div>
                            )}

                            {i === activePlayerIndex && dealStarted && dealFinished && (
                                <div className="bet-buttons">
                                    <button className="clear-button" onClick={handleStand}>Stand</button>
                                    <button className="double-button" onClick={handleDouble}>Double</button>
                                    <button className="confirm-button" onClick={handleHit}>Hit</button>
                                </div>
                            )}
                        </div>
                    </div>
                );
            })}

            {!dealStarted && (
                <div style={{ position: 'absolute', bottom: '16%', width: '100%', textAlign: 'center' }}>
                    <div style={{ color: 'white', fontSize: '14px', fontWeight: 'bold' }}>Place your bet</div>
                </div>
            )}

            <div className="bottom-strip">
                <div className="chip-row">
                    {[5, 10, 25, 50, 100].map((amount) => (
                        <button
                            key={amount}
                            className={`chip ${getChipClass(amount)}`}
                            onClick={() => {
                                if (activePlayerIndex === null || dealStarted) return;

                                setCanQuit(false);

                                const player = players[activePlayerIndex];
                                const currentAmount = bets[activePlayerIndex]?.amount || 0;

                                const maxAllowed = player.balance > 0 ? player.balance : 5;

                                if (currentAmount + amount > maxAllowed) {
                                    alert(`You can't bet more than $${maxAllowed}.`);
                                    return;
                                }

                                setBets(prev => ({
                                    ...prev,
                                    [activePlayerIndex]: {
                                        chip: amount,
                                        amount: currentAmount + amount,
                                        visible: true
                                    }
                                }));
                            }}
                        >
                            {amount}
                        </button>
                    ))}
                </div>

                {activePlayerIndex !== null && players[activePlayerIndex] && (
                    <div className="balance-indicator">
                        Balance: ${players[activePlayerIndex].balance - (bets[activePlayerIndex]?.amount || 0)}
                    </div>
                )}
            </div>

        </div>
    );

}

export default Blackjack;
