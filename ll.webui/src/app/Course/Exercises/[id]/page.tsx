"use client";

import { useState } from "react";
import './page.css';

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
    <div className="exercise-page">
      <h1 className="exercise-title">
        La tercera punta de un lugar al que llaman
      </h1>

      <div className="exercise-rows">
        <div className="exercise-row">
          {topRow.map((word, index) => (
            <button
              key={`top-${index}`}
              className="exercise-word-btn"
              onClick={() => moveWord(word, true)}
            >
              {word}
            </button>
          ))}
        </div>

        <div className="exercise-row">
          {bottomRow.map((word, index) => (
            <button
              key={`bottom-${index}`}
              className="exercise-word-btn"
              onClick={() => moveWord(word, false)}
            >
              {word}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
}
