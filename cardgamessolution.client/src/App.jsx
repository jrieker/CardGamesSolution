import { useState } from 'react';
import './App.css';

function App() {
    const [screen, setScreen] = useState('main'); // main, solitaireAuth, blackjackAuth, blackjackPlayer2
    const [authMode, setAuthMode] = useState('login'); // login or create

    const renderMainMenu = () => (
        <div className="menu">
            <h1>Card Games</h1>
            <button onClick={() => { setAuthMode('login'); setScreen('solitaireAuth'); }}>Solitaire</button>
            <button onClick={() => { setAuthMode('login'); setScreen('blackjackAuth'); }}>Blackjack</button>
        </div>
    );

    const renderAuthScreen = (game) => (
        <div className="auth">
            <div className="back-arrow" onClick={() => setScreen('main')}>&larr;</div>
            <h2>{authMode === 'login' ? 'Login' : 'Create Account'}</h2>
            <input type="text" placeholder="Username" />
            <input type="password" placeholder="Password" />
            <button>{authMode === 'login' ? 'Login' : 'Create Account'}</button>
            <div className="auth-toggle">
                <span className="auth-link" onClick={() => setAuthMode(authMode === 'login' ? 'create' : 'login')}>
                    {authMode === 'login' ? 'Create Account' : 'Log In'}
                </span>
            </div>
        </div>
    );

    return (
        <div className="app-container">
            {screen === 'main' && renderMainMenu()}
            {screen === 'solitaireAuth' && renderAuthScreen('solitaire')}
            {screen === 'blackjackAuth' && renderAuthScreen('blackjack')}
            {screen === 'blackjackPlayer2' && renderAuthScreen('blackjack')}
        </div>
    );
}

export default App;
