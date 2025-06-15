import { useState, useEffect, CSSProperties } from 'react';
import { WordViewModel } from '@/scripts/models';
import { CreateAudio } from '@/scripts/Helpers/SecurityHelper'
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faVolumeUp } from '@fortawesome/free-solid-svg-icons';
import './Flashcards.css'; 

interface FlashcardsProps {text:string, words: WordViewModel[]; onDone: (result: boolean) => void;}

export const Flashcards = ({ text, words, onDone }: FlashcardsProps) => {
    const [courseLength, setCourseLength] = useState<number>(0);  
    const [wordIndex, setWordIndex] = useState<number>(0);
    const [paragraphWords, setParagraphWords] = useState<WordViewModel[]>([]);
    const [paragraphs, setParagraph] = useState<string[]>([]);
    const [paragraphIndex, setParagraphIndex] = useState<number>(0);
    const [loading, setLoading] = useState(false);
    const [showMeaning, setShowMeaning] = useState(false);

    const [feedbackStyle] = useState<CSSProperties>({
      color: "#d63384",
      backgroundColor: "#fff0f6"
    });

    useEffect(() => {

      const sortedParagraphs = text.split('\n\n').sort((a, b) => {
        const aPercentage = Percentage(a, words);
        const bPercentage = Percentage(b, words);
      
        if (aPercentage > bPercentage) return -1;
        if (aPercentage < bPercentage) return 1;
      
        // If both are included or both are not included, sort alphabetically
        return 0;
      });

      var sortedBasedOnAppearance:any = sortBasedOnAppearance(sortedParagraphs[paragraphIndex], words);
      setParagraph(sortedParagraphs);
      setParagraphWords(sortedBasedOnAppearance);
      setCourseLength(sortedBasedOnAppearance.length);
          
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
      setCourseLength(0);
      setWordIndex(0);
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

      setWordIndex(newIndex);
      setParagraphWords(sortBasedOnAppearance(paragraphs[newParagraphIndex], words));
      setCourseLength(paragraphWords.length);
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
        
        return matchingParagraphs[0].replace(new RegExp("(" + word + ")", "g"), word.bold());
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
      {paragraphWords[wordIndex] != null &&
        <div className="container">
            <div className="mt-4">
              <div className="d-flex">
                {Array.from({ length: courseLength }).map((_, index) => (
                  <div
                    key={index}
                    className={`flex-fill me-1 progress-bar ${ index < wordIndex ? "bg-success" : "bg-secondary"}`}
                    style={{height: "20px",marginRight: index < courseLength - 1 ? "2px" : "0",}}
                  ></div>
                ))}
              </div>
            </div>

            <div style={{  display: 'flex', justifyContent: 'center', alignItems: 'center', gap: '20px'}}>
              <div className="mainTxt flip-card" onClick={flipCard}>
                <div className="flip-card-inner">
                    <div className="flip-card-front">
                        <p className="title">{paragraphWords[wordIndex].name}</p>
                    </div>
                    <div className="flip-card-back">
                        <p className="title">{paragraphWords[wordIndex].translation}</p>
                    </div>
                </div>
              </div>
            </div>

            <br></br>
            <button 
              onTouchStart={() => CreateAudio(paragraphWords[wordIndex].id)} 
              onClick={() => CreateAudio(paragraphWords[wordIndex].id)} className='audio'>
                <FontAwesomeIcon icon={faVolumeUp} />
            </button> 
            <span>&nbsp;&nbsp;</span>
            {showMeaning == false && <div dangerouslySetInnerHTML={{ __html: highlightWord(paragraphWords[wordIndex].name) }} />}

            {showMeaning && (paragraphWords[wordIndex].meanings && paragraphWords[wordIndex].meanings.length > 0 ? (
              paragraphWords[wordIndex].meanings.map((meaning) => (
              <div key={wordIndex} style={{ color: 'black', fontFamily: 'fangsong' }}>
                <h3>{meaning.type}</h3>
                <ul>
                  {meaning.definitions && meaning.definitions.length > 0 ? (
                    meaning.definitions.map((definition, defIndex) => (
                      <li key={wordIndex}>{definition}</li>
                    ))
                  ) : (
                    <li>No definitions available</li>
                  )}
                </ul>
              </div>
            ))
          ) : (<p>No meanings available</p> ))}
          
        <div className="feedback-container fixed-bottom" style={feedbackStyle}>
          <button className="mainBtn feedback-button" onClick={() => nextStep(wordIndex + 1)}>
            <span className="chevron">›</span>
              Next
          </button>
        </div>
        </div> }

      {loading && <SpinnerOverlay />}
    </div>
  );
};

export default Flashcards;
