"use client";

import React, { useState, useEffect} from 'react';
import { WordViewModel } from '@/utils/Models/models';
import { CreateAudio } from '@/utils/Security/httpClient'

import './Multiselect.css'; 

interface MultiselectProps { words: WordViewModel[], onDone: () => void;}

export const Multiselect = ({ words, onDone }: MultiselectProps)  => {
    
    const [pairIndex, setPairIndex] = useState<number>(0);
    const [pairs, setPairs] = useState<{ column1: string[]; column2: string[] }[]>([]);
    const [activeWord, setActiveWord] = useState<string | null>(null);
    const [selectedPairs, setSelectedPairs] = useState<{ [key: string]: string }>({});
    const [message, setMessage] = useState<string | null>(null);

    useEffect(() => {

      if(words.length == 0)
        return;

      const frequency = new Map<string, number>();
      for (const word of words.map(kw => kw.translation || ""))
        frequency.set(word, (frequency.get(word) || 0) + 1);

      const maxCount: number = (frequency.size > 0 ? Math.max(...Array.from(frequency.values())) : 1);
      const batchSize: number = Math.round(words.length / maxCount) > 10 ? 10 : Math.round(words.length / maxCount);
      const keyWordsPairs: { column1: string[]; column2: string[] }[] = []

      let allAddedItems = new Set<string>();

      for (let i = 0; i < maxCount; i++) {
          const list: WordViewModel[] = [];
          
          for (let j = 0; j < batchSize; j++) {
              // Find the firstNewItem that meets the conditions
              const firstNewItem = words.find(k => 
                  !keyWordsPairs.some(lp => lp.column1.includes(k.name)) && 
                  !allAddedItems.has(k.translation || "")
              );
      
              if (firstNewItem) {
                  list.push(firstNewItem);
                  allAddedItems.add(firstNewItem.translation || ""); // Track added translations
              } else {
                  break; // If no new item is found, stop the inner loop to avoid empty additions
              }
          }
      
          if (list.length > 0) { // Only add to keyWordsPairs if list has items
              keyWordsPairs.push({
                  column1: list.map(w => w.name),
                  column2: shuffle(list.map(w => w.translation || ""))
              });
          } else {
              // Handle the case where no new items were found in this outer loop iteration
              break;
          }
      }

    setPairs(keyWordsPairs);

    }, []);

    function shuffle<T>(array: string[]): string[] {
  
      const shuffled = [...array];

      for (let i = shuffled.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [shuffled[i], shuffled[j]] = [shuffled[j], shuffled[i]];
      }

      return shuffled;
    }

    const handleWordClick = (word: string, column: number) => {

        if (word === '' || 
        (Object.keys(selectedPairs).includes(word) && column == 1) 
        || (Object.values(selectedPairs).includes(word) && column == 2)) 
        return; // Ignore clicks on empty or already selected words
        
        if (column === 1) {

        let keyWordId: string = words.filter(k => k.name == word)[0].id
        CreateAudio(keyWordId)
        setActiveWord(word);
        setMessage(null);
        } else if (column === 2 && activeWord) {

        const selected: WordViewModel = words.filter(w => w.name == activeWord)[0];
        const correct: boolean = selected.translation === word;
    
        if (correct) {
            setSelectedPairs((prev) => ({ ...prev, [activeWord]: word }));
            setActiveWord(null);
            setMessage('Correct match!');

            if(pairs[pairIndex].column1.length == Object.keys(selectedPairs).length + 1){

            if(pairs[pairIndex + 1] == null){

                onDone();
            }else{

                setSelectedPairs({});
                setPairIndex(pairIndex + 1);
            }
            }
        } else {
            setMessage('Incorrect match! Try again.');
        }
        }
    };

    return (pairs[pairIndex] != null 
        && (<div className='mainTxt keyWord-con'>
              <div style={{ gridColumn: 'span 2' }}>
                <h1>Match the Words</h1>
                <p>Select the matching pairs from the two columns below.</p>
              </div>
              <div>
                {pairs[pairIndex].column1.map((word) => (
                  <div
                    key={word}
                    className='keyWord'
                    style={{
                      cursor: Object.values(selectedPairs).includes(word) ? 'not-allowed' : 'pointer',
                      backgroundColor: Object.keys(selectedPairs).includes(word) ? '#e0e0e0' : activeWord === word ? '#cce4ff' : 'transparent',
                      color: Object.values(selectedPairs).includes(word) ? '#888' : '#000',
                    }}
                    onClick={() => handleWordClick(word, 1)}>
                    {word}
                  </div>
                ))}
              </div>
              <div>
                {pairs[pairIndex].column2.map((word) => (
                  <div
                    key={word}
                    className='keyWord'
                    style={{
                      cursor: Object.values(selectedPairs).includes(word) ? 'not-allowed' : 'pointer',
                      backgroundColor: Object.keys(selectedPairs).includes(word) ? '#e0e0e0' : activeWord === word ? '#cce4ff' : 'transparent',
                      color: Object.values(selectedPairs).includes(word) ? '#888' : '#000',
                    }}
                    onClick={() => handleWordClick(word, 2)}>
                    {word}
                  </div>
                ))}
              </div>
              {message && (
                <div className='keyWord-message'>
                  {message}
                </div>)}
            </div>))}

export default Multiselect;