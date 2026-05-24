"use client";

import { useState, useEffect } from "react";

export default function App() {
  const [topRow, setTopRow] = useState(['known', 'area', 'of', 'a', 'of', 'third']);
  const [bottomRow, setBottomRow] = useState(['final', 'The', 'called', 'place', 'as', 'point', 'The', 'piece', 'an']);

  const moveWord = (word: string, fromTop: boolean) => {
    if (fromTop) {
      setTopRow(topRow.filter(w => w !== word));
      setBottomRow([...bottomRow, word]);
    } else {
      setBottomRow(bottomRow.filter(w => w !== word));
      setTopRow([...topRow, word]);
    }
  };

  return (
    <div
      style={{
        width: '100%',
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        background: 'linear-gradient(135deg, #f3e8ff 0%, #fae8ff 50%, #fdf4ff 100%)',
        padding: '40px 20px',
        gap: '40px'
      }}
    >
      <h1
        style={{
          fontSize: '24px',
          fontWeight: '500',
          color: '#1f2937',
          textAlign: 'center',
          margin: 0
        }}
      >
        La tercera punta de un lugar al que llaman
      </h1>

      <div
        style={{
          display: 'flex',
          flexDirection: 'column',
          gap: '24px',
          alignItems: 'center'
        }}
      >
        <div
          style={{
            display: 'flex',
            gap: '12px',
            flexWrap: 'wrap',
            justifyContent: 'center'
          }}
        >
          {topRow.map((word, index) => (
            <button
              key={`top-${index}`}
              onClick={() => moveWord(word, true)}
              style={{
                background: 'linear-gradient(90deg, #7c3aed 0%, #d946ef 100%)',
                color: 'white',
                border: 'none',
                borderRadius: '12px',
                padding: '14px 24px',
                fontSize: '16px',
                fontWeight: '500',
                cursor: 'pointer',
                boxShadow: '0 4px 12px rgba(124, 58, 237, 0.25)',
                transition: 'all 0.2s ease',
                minWidth: '80px'
              }}
              onMouseEnter={(e) => {
                e.currentTarget.style.transform = 'translateY(-2px)';
                e.currentTarget.style.boxShadow = '0 6px 16px rgba(124, 58, 237, 0.35)';
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.transform = 'translateY(0)';
                e.currentTarget.style.boxShadow = '0 4px 12px rgba(124, 58, 237, 0.25)';
              }}
            >
              {word}
            </button>
          ))}
        </div>

        <div
          style={{
            display: 'flex',
            gap: '12px',
            flexWrap: 'wrap',
            justifyContent: 'center'
          }}
        >
          {bottomRow.map((word, index) => (
            <button
              key={`bottom-${index}`}
              onClick={() => moveWord(word, false)}
              style={{
                background: 'linear-gradient(90deg, #7c3aed 0%, #d946ef 100%)',
                color: 'white',
                border: 'none',
                borderRadius: '12px',
                padding: '14px 24px',
                fontSize: '16px',
                fontWeight: '500',
                cursor: 'pointer',
                boxShadow: '0 4px 12px rgba(124, 58, 237, 0.25)',
                transition: 'all 0.2s ease',
                minWidth: '80px'
              }}
              onMouseEnter={(e) => {
                e.currentTarget.style.transform = 'translateY(-2px)';
                e.currentTarget.style.boxShadow = '0 6px 16px rgba(124, 58, 237, 0.35)';
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.transform = 'translateY(0)';
                e.currentTarget.style.boxShadow = '0 4px 12px rgba(124, 58, 237, 0.25)';
              }}
            >
              {word}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
}