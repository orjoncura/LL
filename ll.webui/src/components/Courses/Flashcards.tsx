import { useState, useEffect } from 'react';
import { CourseViewModel, WordViewModel } from '@/utils/Models/models';
import { POST, CreateAudio } from '@/utils/Security/httpClient'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faVolumeUp, faSquareCaretLeft, faSquareCaretRight, faBoxArchive } from '@fortawesome/free-solid-svg-icons';
import {DeleteCourseWordModel} from '@/utils/Models/models';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';

import '@/components/Feedback/FeedbackView.css';
import './Flashcards.css'; 

interface FlashcardsProps { text:string, words: WordViewModel[], courseId?:number; onDone: (result: boolean) => void;}

export const Flashcards = ({ text, words, courseId, onDone }: FlashcardsProps) => {
    const [flashcards, setFlashcards] = useState<WordViewModel[]>(words);  
    const [flashcardIndex, setFlashcardIndex] = useState<number>(0);
    const [paragraphWords, setParagraphWords] = useState<WordViewModel[]>([]);
    const [paragraphs, setParagraph] = useState<string[]>([]);
    const [paragraphIndex, setParagraphIndex] = useState<number>(0);
    const [currentFlashcard, setCurrentFlashcard] = useState<WordViewModel>();
    const [loading, setLoading] = useState(false);
    const [showMeaning, setShowMeaning] = useState(false);

    useEffect(() => {
      window.scrollTo(0, 0);

      //Remove paragraphs with no matching words.
      const paragraphs = text.split('\n\n').filter(p =>  sortBasedOnAppearance(p, flashcards).length > 0);

      const sortedParagraphs = paragraphs.sort((a, b) => {
        const aPercentage = Percentage(a, flashcards);
        const bPercentage = Percentage(b, flashcards);
      
        if (aPercentage > bPercentage) return -1;
        if (aPercentage < bPercentage) return 1;
      
        // If both are included or both are not included, sort alphabetically
        return 0;
      });

      var sortedBasedOnAppearance:any = sortBasedOnAppearance(sortedParagraphs[paragraphIndex], words);
      setParagraph(sortedParagraphs);
      setParagraphWords(sortedBasedOnAppearance);
      setCurrentFlashcard(sortedBasedOnAppearance[0]);
    }, []);

    function Percentage(paragraph: string, targetWords: WordViewModel[]){
      const targetSet = new Set(targetWords.map(word => word.name.toLowerCase()));
      const words = paragraph.toLowerCase().match(/[a-zA-Z0-9]+/g) || [];
      const uniqueParagraphWords = new Set(words);
      
      if (uniqueParagraphWords.size === 0) return 0;
      
      const matchingCount = Array.from(uniqueParagraphWords).filter(word => 
          targetSet.has(word)
      ).length;
      
      return (matchingCount / uniqueParagraphWords.size) * 100;
    }
    
    const resetAllToDefault = () => {
      setFlashcards([]);
      setFlashcardIndex(0);
      setLoading(false);
      setShowMeaning(false);
      setParagraph([]);
      setParagraphWords([]);
      setParagraphIndex(0);
      onDone(false)
    };
    
    const nextStep = (newIndex: number) => {

      let newParagraphIndex = paragraphIndex;
      if(paragraphWords[newIndex] == null){

        newIndex = 0;
        newParagraphIndex = paragraphIndex + 1;
      }

      setParagraphIndex(newParagraphIndex);
      
      if(paragraphs[newParagraphIndex] == null)
        resetAllToDefault();

      setFlashcardIndex(newIndex);
      setParagraphWords(sortBasedOnAppearance(paragraphs[newParagraphIndex], flashcards));
      setCurrentFlashcard(paragraphWords[newIndex]);
    }
    
    const archive = (index:number) => {
          
          if(courseId === null) return;

          setLoading(true); 
          
          const word: WordViewModel = paragraphWords[index]

          const data: DeleteCourseWordModel = {
            "courseId": courseId,
            "wordId": word.id
          };

          POST('/Course/DeleteCourseWord', JSON.stringify(data))                
          .then(isSuccessfull => { 

              if(isSuccessfull) {
                  var wordsList = paragraphWords.filter(p => p.id !== word.id);
                  setFlashcards(flashcards.filter(f => f.id !== word.id));
                  setParagraphWords(wordsList);

                  if(wordsList[index] != null) 
                    setCurrentFlashcard(wordsList[index]) 
                  else
                    resetAllToDefault();
              }
              
            }).finally(() => {setLoading(false)});
    }

    const flipCard = () => {

      const cardElement = document.querySelector('.flip-card-inner');
    
      if (cardElement != null && !cardElement.classList.contains('flipped')) {
        cardElement.classList.toggle('flipped');
        setShowMeaning(true);

      }else if(cardElement != null){

        cardElement.classList.remove('flipped');
        setShowMeaning(false);
      }

    };
    
    function highlightWord(word: string): string {
        const trimmedText = text.trim();
        
        if (trimmedText === '') 
          return "";

        const matchingParagraphs = trimmedText.split('\n\n').filter(t => t.includes(word));
        
        if (matchingParagraphs.length === 0) 
          return "";
        
        let paragraph = matchingParagraphs[0];

        const escapedWord = word.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
        const regex = new RegExp(`\\b${escapedWord}\\b`, 'g');

        return paragraph.replace(regex, `<b>${word}</b>`); 
    }

    function sortBasedOnAppearance(text: string, wordsList:WordViewModel[]): WordViewModel[] {

      if(text == null || wordsList.length == 0) return [];

      wordsList = wordsList.filter(w => text.includes(w.name));

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

  return (
    <div>
      {currentFlashcard != null &&
        <div className="container">
            <div className="mt-4">
              <div className="d-flex">
                {Array.from({ length: paragraphWords.length }).map((_, index) => (
                  <div
                    key={index}
                    className={`flex-fill me-1 progress-bar ${ index < flashcardIndex ? "bg-success" : "bg-secondary"}`}
                    style={{height: "20px",marginRight: index < paragraphWords.length  - 1 ? "2px" : "0",}}
                  ></div>
                ))}
              </div>
            </div>

            <div style={{  display: 'flex', justifyContent: 'center', alignItems: 'center', gap: '20px'}}>
              <div className="mainTxt flip-card" onClick={flipCard}>
                <div className="flip-card-inner">
                    <div className="flip-card-front">
                        <p className="title">{currentFlashcard.name}</p>
                    </div>
                    <div className="flip-card-back">
                        <p className="title">{currentFlashcard.translation}</p>
                    </div>
                </div>
              </div>
            </div>

            <br></br>

            <span>&nbsp;&nbsp;</span>
            {showMeaning == false && <div dangerouslySetInnerHTML={{ __html: highlightWord(currentFlashcard.name) }} style={{marginBottom: "40%"}} />}

            {showMeaning && (currentFlashcard.meanings && currentFlashcard.meanings.length > 0 ? (
              currentFlashcard.meanings.map((meaning) => (
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
          
          <div className="feedback-container fixed-bottom" style={{justifyContent:"flex-end"}}>
            <div className="row" >
                <div className='buttonDiv'>
                  <button className="icon-button mb-1" onClick={() => nextStep(flashcardIndex - 1)}>
                    <FontAwesomeIcon icon={faSquareCaretLeft} />
                  </button>
                  <div className="button-label">Back</div>
                </div>

                {courseId != null && 
                <div className='buttonDiv'>
                  <button className="icon-button mb-1" onClick={() => archive(flashcardIndex)}>
                    <FontAwesomeIcon icon={faBoxArchive} />
                  </button>
                  <div className="button-label">Archive</div>
                </div>}

                <div className='buttonDiv'>
                  <button className="icon-button mb-1" onClick={() => CreateAudio(currentFlashcard.id)}>
                    <FontAwesomeIcon icon={faVolumeUp} />
                  </button>
                  <div className="button-label">Audio</div>
                </div>

                <div className='buttonDiv'>
                  <button className="icon-button mb-1" onClick={() => nextStep(flashcardIndex + 1)}>
                    <FontAwesomeIcon icon={faSquareCaretRight} />
                  </button>
                  <div className="button-label">Next</div>
                </div>
            </div>
          </div>
        </div> }

      {loading && <SpinnerOverlay />}
    </div>
  );
};

export default Flashcards;
