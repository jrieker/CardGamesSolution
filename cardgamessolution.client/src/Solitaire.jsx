import React from 'react';
import './Solitaire.css';

function Solitaire({ username }) {
    // I stored username in here temporarily to use to connect to backend, that's why it displays at the top left of the screen. 
    // When you design the solitaire page I would remove that whenever you decide to connect to the backend with the username
    return (
        <div className="table-screen">
            <div style={{
                position: 'absolute',
                top: '10px',
                left: '10px',
                color: 'white',
                fontSize: '18px' 
            }}>
                {username} 
            </div>
            {}
        </div>
    );
}

export default Solitaire;
