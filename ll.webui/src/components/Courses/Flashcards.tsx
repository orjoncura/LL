import { useState, useRef, useEffect, ReactNode, ReactElement } from 'react';
import { WordViewModel } from '@/utils/Models/models';
import { POST, CreateAudio } from '@/utils/Security/httpClient'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faVolumeUp, faSquareCaretLeft, faSquareCaretRight, faBoxArchive } from '@fortawesome/free-solid-svg-icons';
import { DeleteCourseWordModel } from '@/utils/Models/models';

import { motion } from "framer-motion";
import FeedbackView from '@/components/Feedback/FeedbackView';
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
    const [isDragging, setIsDragging] = useState(false);
    const [loading, setLoading] = useState(false);
    const [showMeaning, setShowMeaning] = useState(false);
    const feedbackViewRef = useRef<any>(null); 
    const [fbTitle, setfbhTitle] = useState('');
    const [fbBody, setfbhBody] = useState('');
    const [onFeedBackViewClick, setOnFeedBackViewClick] = useState<(() => void) | undefined>(undefined);

    const showFeedback = (title: string, body:string, onClick?: Function) => {
        if (feedbackViewRef.current) {
            setfbhTitle(title);
            setfbhBody(body);
            setOnFeedBackViewClick(() => onClick); 
            feedbackViewRef.current.open();
        }
    };

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

      //Back to the parent page.
      const handlePopState = () => {
        onDone(false); 
      };

      window.addEventListener("popstate", handlePopState);
      return () => window.removeEventListener("popstate", handlePopState);
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
    
    const navigateToFlashcardByIndex = (newIndex: number) => {

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
      setShowMeaning(false);
    }
    
    const archive = (index:number) => {
          
          if(courseId === null) return;

          const word: WordViewModel = paragraphWords[index]

          let fun = () => {
            setLoading(true); 
            
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
          };

        showFeedback("Warning", "Would you like to archive '" + word.name + "'", fun);
    }
    
    function highlightWord(word:string, wordIndex: number): ReactElement {

      if(!word) return <p></p>;

        const escapedWord = word.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
        const wordKey = "pw_" + wordIndex;

        if(escapedWord == paragraphWords[flashcardIndex].name)
          return <b key={wordKey} style={{ marginRight: 2 }}>{word}</b>;

        // Check if any of the extracted words match the paragraphWords list
        if(paragraphWords.some(w => w.name == escapedWord)){
             
          const index = paragraphWords.findIndex(p => p.name === word); 
          return <span key={wordKey} style={{ textDecorationLine: 'underline', 
            WebkitTextDecorationLine: 'underline', cursor:'pointer', marginRight: 2}} 
                onClick={() => navigateToFlashcardByIndex(index)}>{word}</span>
        }

        return <p key={wordKey} style={{ marginRight: 2 }}>{word}</p>;
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
            
            <div className='card-con'>
              {paragraphWords.map((card, index) => 
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
                  <div className={`card-inner mainTxt ${showMeaning ? "flipped" : ""}`}>
                    <div className="card-face">{card.name}</div>
                    <div className="card-face card-back">{card.translation}</div>
                  </div>
                </motion.div>)
                : null)}
            </div>

            <br></br>
 
            {showMeaning == false && <div style={{marginBottom: "40%", display: 'inline-flex', flexWrap: 'wrap'}}>
              {text.trim().split(' ').map((word, index) => highlightWord(word, index))}
            </div>}

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
          
          <div className="flashcardCon fixed-bottom" style={{justifyContent:"flex-end"}}>
            <div className="row" style={{marginTop: "-5px"}} >
                <div className='buttonDiv'>
                  <button className="icon-button mb-1" onClick={() => navigateToFlashcardByIndex(flashcardIndex - 1)}>
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
                  <button className="icon-button mb-1" onClick={() => navigateToFlashcardByIndex(flashcardIndex + 1)}>
                    <FontAwesomeIcon icon={faSquareCaretRight} />
                  </button>
                  <div className="button-label">Next</div>
                </div>
            </div>
          </div>
        </div> }

      {loading && <SpinnerOverlay />}    
      <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody} onClick={onFeedBackViewClick} showCloseBtn={true} />
    </div>
  );
};

export default Flashcards;
