import { useState } from 'react';
import './App.css';

function App() {
    const [screen, setScreen] = useState('main');
    const [authMode, setAuthMode] = useState('login');

    const renderMainMenu = () => (
        <div className="menu">
            <h1>Card Games</h1>
            <button className="menu-button" onClick={() => { setAuthMode('login'); setScreen('solitaireAuth'); }}>Solitaire</button>
            <button className="menu-button" onClick={() => { setAuthMode('login'); setScreen('blackjackAuth'); }}>Blackjack</button>
        </div>
    );

    const renderAuthScreen = (game) => (
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

                        const result = await response.json();

                        if (result.success) {
                            alert(`${authMode === 'login' ? 'Logged in' : 'Account created'} successfully!`);
                        } else {
                            alert(result.message || 'Failed. Please try again.');
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

    const backgroundClass =
        screen === 'main' ? 'main-screen' :
            screen === 'solitaireAuth' ? 'solitaire-screen' :
                'blackjack-screen';

    return (
        <div className={`app-container screen ${backgroundClass}`}>
            {screen === 'main' && renderMainMenu()}
            {screen === 'solitaireAuth' && renderAuthScreen('solitaire')}
            {screen === 'blackjackAuth' && renderAuthScreen('blackjack')}
            {screen === 'blackjackPlayer2' && renderAuthScreen('blackjack')}
        </div>
    );
}

export default App;
