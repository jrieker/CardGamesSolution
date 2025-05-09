import { useState } from 'react';
import './App.css';
import Blackjack from './Blackjack';
import Solitaire from './Solitaire';

function App() {
    const [screen, setScreen] = useState('main');
    const [authMode, setAuthMode] = useState('login');
    const [playerList, setPlayerList] = useState([]);
    const [solitaireUsername, setSolitaireUsername] = useState('');

    const handleAddPlayer = (userObj) => {
        setPlayerList(prev => {
            if (prev.length >= 6) {
                alert("Maximum of 6 players allowed.");
                return prev;
            }

            return prev.some(u => u.userId === userObj.userId)
                ? prev
                : [...prev, userObj];
        });
    };


    const handleRemovePlayer = (username) => {
        setPlayerList(prev => prev.filter(player => player.username !== username));
    };

    const renderMainMenu = () => (
        <div className="menu">
            <h1>Card Games</h1>
            <button className="menu-button" onClick={() => { setAuthMode('login'); setScreen('solitaireAuth'); }}>Solitaire</button>
            <button className="menu-button" onClick={() => { setAuthMode('login'); setScreen('blackjackAuth'); }}>Blackjack</button>
        </div>
    );

    const renderAuthScreen = (game) => {
        const authBox = (
            <div className="auth">
                <div className="back-arrow" onClick={() => setScreen('main')}>&larr;</div>
                <h2>{game === 'solitaire' ? 'Solitaire' : 'Blackjack'}</h2>
                <input type="text" placeholder="Username" />
                <input type="password" placeholder="Password" />
                <button
                    className="auth-button"
                    onClick={async () => {
                        const username = document.querySelector('input[placeholder="Username"]').value;
                        const password = document.querySelector('input[placeholder="Password"]').value;
                        const endpoint = authMode === 'login' ? 'login' : 'register';

                        try {
                            const response = await fetch(`/api/user/${endpoint}`, {
                                method: 'POST',
                                headers: { 'Content-Type': 'application/json' },
                                body: JSON.stringify({ username, password })
                            });

                            if (!response.ok) {
                                const errorResult = await response.json();
                                alert(errorResult.message || "Login failed.");
                                return;
                            }

                            const result = await response.json();

                            if (result.success) {
                                alert(`${authMode === 'login' ? 'Logged in' : 'Account created'} successfully!`);

                                if (game === 'blackjack' && result.username) {
                                    const userObj = {
                                        userId: result.userId,
                                        username: result.username,
                                        password,
                                        balance: result.balance || 0,
                                        wins: result.wins || 0,
                                        losses: result.losses || 0
                                    };
                                    handleAddPlayer(userObj);
                                } else if (game === 'solitaire' && result.username) {
                                    setSolitaireUsername(result.username);
                                    setScreen('solitaireGame');
                                }
                            }
                        } catch (error) {
                            console.error('Error:', error);
                            alert('Server error. See console.');
                        }
                    }}
                >
                    {authMode === 'login' ? 'Login' : 'Create Account'}
                </button>
                <div className="auth-toggle">
                    <span className="auth-link" onClick={() => setAuthMode(authMode === 'login' ? 'create' : 'login')}>
                        {authMode === 'login' ? 'Need an account? Create one' : 'Already have an account? Log in'}
                    </span>
                </div>
            </div>
        );

        if (game === 'blackjack') {
            return (
                <div className="auth-layout">
                    {authBox}
                    <div className="playerlist-box">
                        <h3>Playerlist</h3>
                        <div className="playerlist-placeholder">
                            {playerList.length === 0 ? (
                                <p>(Empty)</p>
                            ) : (
                                playerList.map((player, index) => (
                                    <div className="playerlist-item" key={index}>
                                        <span>{player.username}</span>
                                        <button className="remove-button" onClick={() => handleRemovePlayer(player.username)}>X</button>
                                    </div>
                                ))
                            )}
                        </div>
                        <button
                            className="play-button"
                            onClick={async () => {
                                const res = await fetch("/api/blackjack/start", {
                                    method: 'POST',
                                    headers: { 'Content-Type': 'application/json' },
                                    body: JSON.stringify(playerList)
                                });
                                if (res.ok) {
                                    setScreen('blackjackGame');
                                }
                            }}
                        >
                            Play
                        </button>
                    </div>
                </div>
            );
        }

        return authBox;
    };

    const backgroundClass =
        screen === 'main' ? 'main-screen' :
            screen === 'solitaireAuth' ? 'solitaire-screen' :
                screen === 'blackjackAuth' ? 'blackjack-screen' :
                    '';

    return (
        <div className={`app-container screen ${backgroundClass}`}>
            {screen === 'main' && renderMainMenu()}
            {screen === 'solitaireAuth' && renderAuthScreen('solitaire')}
            {screen === 'blackjackAuth' && renderAuthScreen('blackjack')}
            {screen === 'blackjackGame' && <Blackjack loggedInPlayers={playerList} />}
            {screen === 'solitaireGame' && <Solitaire username={solitaireUsername} />}
        </div>
    );
}

export default App;
