import React, { useState, useMemo } from 'react';
import './Leaderboard.css';

function Leaderboard({ players }) {
    const [sortConfig, setSortConfig] = useState({ key: 'id', direction: 'ascending' });

    const sortedPlayers = useMemo(() => {
        if (!players) return [];
        // Create a copy of players array to sort
        const sortableItems = [...players];
        const { key, direction } = sortConfig;

        sortableItems.sort((a, b) => {
            let aValue = a[key];
            let bValue = b[key];

            // Handle case-insensitive comparison for strings
            if (typeof aValue === 'string') {
                aValue = aValue.toLowerCase();
                bValue = bValue.toLowerCase();
            }

            if (aValue < bValue) {
                return direction === 'ascending' ? -1 : 1;
            }
            if (aValue > bValue) {
                return direction === 'ascending' ? 1 : -1;
            }
            return 0;
        });

        return sortableItems;
    }, [players, sortConfig]);

    const requestSort = (key) => {
        setSortConfig(prevConfig => {
            if (prevConfig.key === key) {
                // Toggle direction if same key is clicked
                const newDirection = prevConfig.direction === 'ascending' ? 'descending' : 'ascending';
                return { key, direction: newDirection };
            }
            // Default to ascending when new key is selected
            return { key, direction: 'ascending' };
        });
    };

    return (
        <div className="leaderboard-container">
            <h2>Leaderboard</h2>
            <table className="leaderboard-table">
                <thead>
                    <tr>
                        <th onClick={() => requestSort('id')}>
                            Player ID {sortConfig.key === 'id' ? (sortConfig.direction === 'ascending' ? '▲' : '▼') : ''}
                        </th>
                        <th onClick={() => requestSort('name')}>
                            Name {sortConfig.key === 'name' ? (sortConfig.direction === 'ascending' ? '▲' : '▼') : ''}
                        </th>
                        <th onClick={() => requestSort('score')}>
                            Score {sortConfig.key === 'score' ? (sortConfig.direction === 'ascending' ? '▲' : '▼') : ''}
                        </th>
                        <th onClick={() => requestSort('balance')}>
                            Balance {sortConfig.key === 'balance' ? (sortConfig.direction === 'ascending' ? '▲' : '▼') : ''}
                        </th>
                        <th onClick={() => requestSort('wins')}>
                            Wins {sortConfig.key === 'wins' ? (sortConfig.direction === 'ascending' ? '▲' : '▼') : ''}
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {sortedPlayers.map((player) => (
                        <tr key={player.id}>
                            <td>{player.id}</td>
                            <td>{player.name}</td>
                            <td>{player.score}</td>
                            <td>{player.balance}</td>
                            <td>{player.wins}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default Leaderboard;
