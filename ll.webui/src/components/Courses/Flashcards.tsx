import { useState, useEffect, ReactElement } from 'react';
import { WordViewModel } from '@/utils/Models/models';
import { DeleteCourseWord, SetWordDifficulty, StreamAudio} from '@/utils/Controllers/CourseController'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faVolumeUp, faSquareCaretLeft, faSquareCaretRight, faSmile, faSadCry, faSadTear } from '@fortawesome/free-solid-svg-icons';
import { ImportanceRatingEnum } from '@/utils/Models/Enums';

import { motion } from "framer-motion";
import '@/components/Feedback/FeedbackView.css';
import './Flashcards.css'; 

interface FlashcardsProps { text:string, words: WordViewModel[], moduleId?:string; onDone: (result: boolean) => void;}

export const Flashcards = ({ text, words, moduleId, onDone }: FlashcardsProps) => {
    const [flashcards, setFlashcards] = useState<WordViewModel[]>(words);  
    const [flashcardIndex, setFlashcardIndex] = useState<number>(0);
    const [isDragging, setIsDragging] = useState(false);
    const [showMeaning, setShowMeaning] = useState(false);

    useEffect(() => {
     
      var sortedBasedOnAppearance:any = sortBasedOnAppearance(text, words);
      setFlashcards(sortedBasedOnAppearance)
    }, []);
    
    const navigateToFlashcardByIndex = (newIndex: number) => {

      if(newIndex >= flashcards.length){

        onDone(true);
      }else{

        setFlashcardIndex(newIndex);
        setShowMeaning(false);
      }
    }
        
    function highlightWord(word:string): ReactElement {

      if(!word) return <p></p>;

        const escapedWord = word.replace(/[^a-zA-Z0-9áéíóúÁÉÍÓÚüÜñÑ ]/g, '') ;

        if(escapedWord == flashcards[flashcardIndex].name)
          return <b style={{ marginRight: 2 }}>{word}</b>;

        // Check if any of the extracted words match the paragraphWords list
        if(flashcards.some(w => w.name == escapedWord)){
             
          const flashcard = flashcards.filter(w => w.name == escapedWord)[0];

          if(word.length > 1 && flashcard.translation != null && flashcard.translation?.length > 1 && flashcard.translation != word)
            return <span  className="highlight-word"
                          style={{ textDecorationLine: 'underline', 
                          WebkitTextDecorationLine: 'underline',
                          cursor:'pointer', marginRight: 2}}
                          onClick={() => StreamAudio(flashcard.id)}>
                      <button className="tooltip-button">{word}</button>
                      <div className="tooltip-content">{flashcard.translation}</div>
                  </span>
        }

        return <p style={{ marginRight: 2 }}>{word}</p>;
    }

    function sortBasedOnAppearance(text: string, wordsList:WordViewModel[]): WordViewModel[] {

      if(text == null || wordsList.length == 0) return [];

      wordsList = wordsList.filter(w => text.includes(w.name) && w.name.length > 0 && w.translation && w.translation.length > 0);

      // Normalize the sample text: split into words and convert to lowercase without punctuation
      const normalizedText = text.split(/\s+/).map(word => word.trim().toLowerCase()).filter(word => word.length > 0);

      // Create an array of objects with each word's position in the text
      const wordIndices = wordsList.map(word => {
          const lowerWord = word.name.trim().toLowerCase();
          // Find the first occurrence index or a large number if not found
          const index = normalizedText.indexOf(lowerWord);
          return { originalWord: word, index };
      });

      // Sort based on the index; handle missing words by placing them last
      const sorted = [...wordIndices]
          .sort((a, b) => {
              if (a.index === -1 && b.index === -1) return 0;
              if (a.index === -1) return 1;
              if (b.index === -1) return -1;
              return a.index - b.index;
          })
          .map(item => item.originalWord);

        return sorted;
    };

    function stripHtml(html: string): string {
      const parser = new DOMParser();
      const doc = parser.parseFromString(html, "text/html");
      return doc.body.textContent || "";
    }

    function findSentencesByWord(html: string): ReactElement[] {

      const word = flashcards[flashcardIndex].name;
      const plainText = stripHtml(html);

      //Split into sentences
      const sentences = plainText
        .split(/(?<=[.!?])\s+/)
        .map(s => s.trim())
        .filter(s => s.length > 0);

      //Find sentences containing the word
      const regex = new RegExp(`\\b${word}\\b`, "i");
      const matchingSentences = sentences.filter(s => regex.test(s));

      //Join with blank line
      const tx = matchingSentences.join("\n\n");

      //Highlight every word in the final text
      return tx.split(/\s+/).map((w, i) => (
        <span key={i}>
          {highlightWord(w)}{" "}
        </span>))
    }
  
    const handleFeedback = (importanceRating: ImportanceRatingEnum): void => {

        if(importanceRating == ImportanceRatingEnum.Low){

        const word: WordViewModel = flashcards[flashcardIndex]

        DeleteCourseWord(decodeURIComponent(moduleId || "" ), word.id)                
        .then(isSuccessfull => { 

            if(isSuccessfull) {
                var wordsList = flashcards.filter(p => p.id !== word.id);
                setFlashcards(wordsList);

                if(wordsList[flashcardIndex] != null) 
                  navigateToFlashcardByIndex(flashcardIndex);
                else onDone(true);
            }
        });

      }else{

        navigateToFlashcardByIndex(flashcardIndex + 1);
      }

      if(flashcards[flashcardIndex] == null)
        return;

      SetWordDifficulty(importanceRating, flashcards[flashcardIndex].id);
    };

  return (
    <div>
      {flashcards[flashcardIndex] != null &&
        <div className="container">
            <div className="mt-4">
              <div className="d-flex">
                {Array.from({ length: flashcards.length }).map((_, index) => (
                  <div
                    key={index}
                    className={`flex-fill me-1 progress-bar ${ index < flashcardIndex ? "bg-success" : "bg-secondary"}`}
                    style={{height: "20px",marginRight: index < flashcards.length  - 1 ? "2px" : "0",}}
                  ></div>
                ))}
              </div>
            </div>
            
            <div className='card-con'>
              {flashcards.map((card, index) => 
                index >= flashcardIndex ?
                  (<motion.div
                    key={card.id}
                    className={`flip-card ${index == flashcardIndex ? "main" : ""}`}
                    drag
                    dragElastic={1}
                    style={{ zIndex: flashcards.length - index  }}
                    onClick={() => !isDragging && setShowMeaning(!showMeaning)}
                    onDragStart={() => setIsDragging(true)}
                    onDragEnd={() => {
                      setIsDragging(false);
                      navigateToFlashcardByIndex(flashcardIndex + 1);
                    }}
                    whileTap={{ scale: 1.1 }}
                  >
                  <div className={`card-inner mainTxt ${showMeaning && index == flashcardIndex ? "flipped" : ""}`}>
                    <div className="card-face">{card.name}</div>
                    <div className="card-face card-back">{card.translation}</div>
                  </div>
                </motion.div>)
                : null)}
            </div>

            <br></br>
 
            {showMeaning == false && <div className='sen'>
              {findSentencesByWord(text)}
            </div>}

            {showMeaning && (flashcards[flashcardIndex].meanings && flashcards[flashcardIndex].meanings.length > 0 ? (
              flashcards[flashcardIndex].meanings.map((meaning) => (
              <div key={flashcardIndex} style={{ color: 'black', fontFamily: 'fangsong' }}>
                <h3>{meaning.type}</h3>
                <ul>
                  {meaning.definitions && meaning.definitions.length > 0 ? (
                    meaning.definitions.map((definition, defIndex) => (
                      <li key={flashcardIndex}>{definition}</li>
                    ))
                  ) : (
                    <li>No definitions available</li>
                  )}
                </ul>
              </div>
            ))
          ) : (<p>No meanings available</p> ))}
          
        <div className="flashcardCon fixed-bottom" style={{ justifyContent: "flex-end" }}>
          <div className="row" style={{ marginTop: "-5px" }}>

            {!showMeaning ? (
              <>
                <div className='buttonDiv'>
                  <button className="icon-button mb-1" onClick={() => navigateToFlashcardByIndex(flashcardIndex - 1)}>
                    <FontAwesomeIcon icon={faSquareCaretLeft} />
                  </button>
                  <div className="button-label">Back</div>
                </div>

                <div className='buttonDiv'>
                  <button className="icon-button mb-1" onClick={() => StreamAudio(flashcards[flashcardIndex].id)}>
                    <FontAwesomeIcon icon={faVolumeUp} />
                  </button>
                  <div className="button-label">Audio</div>
                </div>

                <div className='buttonDiv'>
                  <button className="icon-button mb-1" onClick={() => navigateToFlashcardByIndex(flashcardIndex + 1)}>
                    <FontAwesomeIcon icon={faSquareCaretRight} />
                  </button>
                  <div className="button-label">Next</div>
                </div>
              </>
            ) : (
              <>
                <div className='buttonDiv'>
                  <button className="icon-button mb-1" onClick={() => handleFeedback(ImportanceRatingEnum.Low)}>
                    <FontAwesomeIcon icon={faSmile} />
                  </button>
                  <div className="button-label">Easy</div>
                </div>
                
                <div className='buttonDiv'>
                  <button className="icon-button mb-1" onClick={() => handleFeedback(ImportanceRatingEnum.Medium)}>
                    <FontAwesomeIcon icon={faSadTear} />
                  </button>
                  <div className="button-label">Medium</div>
                </div>
                
                <div className='buttonDiv'>
                  <button className="icon-button mb-1" onClick={() => handleFeedback(ImportanceRatingEnum.High)}>
                    <FontAwesomeIcon icon={faSadCry} />
                  </button>
                  <div className="button-label">Hard</div>
                </div>
              </>
            )}

          </div>
        </div>

        </div> }
    </div>
  );
};

export default Flashcards;
