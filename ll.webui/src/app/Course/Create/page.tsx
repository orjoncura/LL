"use client";
import React, {useState, useEffect, useRef, CSSProperties} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import ModalView from '@/components/Modal/ModalView';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import Flashcards from '@/components/Courses/Flashcards';
import { GET, POST, CreateAudio } from '@/scripts/Helpers/SecurityHelper'
import {CourseRequestModel, WordViewModel} from '@/scripts/models';
import './page.css'; 

export default function CreateSeminar() {
    const modalRef = useRef<any>(null); 

    const [showKeyWords, setShowKeyWords] = useState(false);
    const [keyWords, setKeyWords] = useState<WordViewModel[]>([]);
    const [pairIndex, setPairIndex] = useState<number>(0);
    const [pairs, setPairs] = useState<{ column1: string[]; column2: string[] }[]>([]);
    const [activeWord, setActiveWord] = useState<string | null>(null);
    const [selectedPairs, setSelectedPairs] = useState<{ [key: string]: string }>({});
    const [message, setMessage] = useState<string | null>(null);
    const [wordViewModels, setWordViewModels] = useState<WordViewModel[]>([]);    
    const [loading, setLoading] = useState(false);
    const [modalTitle, setModalTitle] = useState('');
    const [modalBody, setModalBody] = useState('');
    const [showCourse, setShowCourse] = useState(false);
    const [onModalClick, setOnModalClick] = useState<(() => void) | undefined>(undefined);
    const [courseText, setCourseText] = useState<string>('');   

    const [feedbackStyle] = useState<CSSProperties>({
      color: "#d63384",
      backgroundColor: "#fff0f6"
    });

    const languageFromId = 2;
    const languageToId = 1;

    let hasFetchedData = false;
    
    useEffect(() => {

      if(hasFetchedData == false){
        hasFetchedData = true;

        GET('/Course/GetKeyWords?languageId=' + languageFromId)
        .then((words: WordViewModel[]) => {
  
          if(words == null || words == undefined)
            return;
  
          setKeyWords(words);
        });
      }
    }, []);
    
    const openModal = (title: string, body:string, onClick?: Function) => {
      if (modalRef.current) {

          setModalTitle(title);
          setModalBody(body);
          setOnModalClick(() => onClick); 

          modalRef.current.openModal(); // Call openModal from the Example component
      }
    };
    
    const handleWordClick = (word: string, column: number) => {

      if (word === '' || 
        (Object.keys(selectedPairs).includes(word) && column == 1) 
        || (Object.values(selectedPairs).includes(word) && column == 2)) 
        return; // Ignore clicks on empty or already selected words
      
      if (column === 1) {

        let keyWordId: number = keyWords.filter(k => k.name == word)[0].id
        CreateAudio(keyWordId)
        setActiveWord(word);
        setMessage(null);
      } else if (column === 2 && activeWord) {

        const selected: WordViewModel = keyWords.filter(w => w.name == activeWord)[0];
        const correct: boolean = selected.translation === word;
  
        if (correct) {
          setSelectedPairs((prev) => ({ ...prev, [activeWord]: word }));
          setActiveWord(null);
          setMessage('Correct match!');

          if(pairs[pairIndex].column1.length == Object.keys(selectedPairs).length + 1){

            if(pairs[pairIndex + 1] == null){

              setShowKeyWords(false);
              setShowCourse(wordViewModels[0] != null);
              setLoading(wordViewModels[0] == null);
              setPairs([]);
              setSelectedPairs({});
              setPairIndex(0);
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

    const handleSubmit = async () => {

        try {

            if(courseText.length == 0)
            {
              openModal("Error", "Text can not be empty.");
              return;
            }

            var textCleaned = courseText.replaceAll(/[\r\n]+/g, " ");
            startCourse(textCleaned);

            const courseRequestModel: CourseRequestModel = {
              "text": textCleaned,
              "languageFromId": languageFromId,
              "languageToId": languageToId
            };

            const wordViewModels: WordViewModel[] =  await POST('/Course/Create', JSON.stringify(courseRequestModel));
            if(wordViewModels == null || wordViewModels == undefined){
              setLoading(false);
              return;
            }
            
            for (const word of wordViewModels.sort((a, b) => a.importanceRatingId > b.importanceRatingId ? 1 : -1)) {
              
              if(wordViewModels.some(w => w.name == word.name) == false)
                wordViewModels.push(word);
 
              if (showKeyWords == false && loading == true) {
                  setLoading(false);
                  startCourse(textCleaned);
              }  
            }
          } catch (error) {

            setLoading(false);
            console.error('Error making API call:', error);
        }
    };

    const startCourse = (text: string) => {

      if(showCourse == false && showKeyWords == false){

        let wordList = text.split(" ");
        let kw: WordViewModel[] = keyWords.filter(w => wordList.includes(w.name) && w.importanceRatingId == 1);

        if(kw.length > 0){

          const frequency = new Map<string, number>();
          for (const word of kw.map(kw => kw.translation || ""))
            frequency.set(word, (frequency.get(word) || 0) + 1);

          const maxCount: number = (frequency.size > 0 ? Math.max(...Array.from(frequency.values())) : 1);
          const batchSize: number = Math.round(kw.length / maxCount);
          const keyWordsPairs: { column1: string[]; column2: string[] }[] = []

          let allAddedItems = new Set<string>();

          for (let i = 0; i < maxCount; i++) {
              const list: WordViewModel[] = [];
              
              for (let j = 0; j < batchSize; j++) {
                  // Find the firstNewItem that meets the conditions
                  const firstNewItem = kw.find(k => 
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
        }
        
        const words = keyWords.filter(w => wordList.includes(w.name) && w.importanceRatingId == 2);

        setShowKeyWords(kw.length > 0);
        setWordViewModels(words);
        setLoading(kw.length == 0);
      }
    }

    function shuffle<T>(array: string[]): string[] {
  
      const shuffled = [...array];

      for (let i = shuffled.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [shuffled[i], shuffled[j]] = [shuffled[j], shuffled[i]];
      }

      return shuffled;
    }
      
    return (
      <div>

        <Navbar /> 

        <div className="container-flex">
          <div className="container mt-4" style={{ marginBottom: "25%" }}>
  
            {showCourse == false && showKeyWords == false && ( 
            <div>
                <div  style={{textAlign: 'center'}}>
                  <b className='mainTxt'>Transform your ideas into a unique and impactful learning experience</b>
                  <label>We empower you to leverage provided input to create a customized educational journey that aligns perfectly
                        with your specific goals and needs.</label>
                </div> 

                <br/><br/><br/>
                <textarea
                        value={courseText}
                        onChange={e => setCourseText(e.target.value)}
                        placeholder="Please enter your text here..."
                        onKeyDown={(e) => {
                          if (e.key === 'Enter') {
                            e.preventDefault();
                              handleSubmit();
                          }}}

                        className='mainTxt'
                        rows={10}
                        cols={50}
                        style={{ marginBottom: '10px', width: '100%' }}
                      />
                <div className="feedback-container fixed-bottom" style={feedbackStyle}>
                    <button className="mainBtn feedback-button" onClick={handleSubmit}>
                      <span className="chevron">›</span> Confirm 
                    </button>
                </div>
            </div>
            )}

            {showKeyWords && (
            <div className='mainTxt' style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px', maxWidth: '400px', margin: '0 auto', textAlign: 'center', padding: '16px' }}>
              <div style={{ gridColumn: 'span 2', marginBottom: '16px' }}>
                <h1 style={{ fontSize: '24px', fontWeight: 'bold' }}>Match the Words</h1>
                <p style={{ fontSize: '16px', color: '#555' }}>Select the matching pairs from the two columns below.</p>
              </div>
              <div>
                {pairs[pairIndex].column1.map((word) => (
                  <div
                    key={word}
                    style={{
                      padding: '8px',
                      marginBottom: '8px',
                      border: '1px solid #ccc',
                      borderRadius: '4px',
                      cursor: Object.keys(selectedPairs).includes(word) ? 'not-allowed' : 'pointer',
                      backgroundColor: Object.keys(selectedPairs).includes(word) ? '#e0e0e0' : activeWord === word ? '#cce4ff' : '#fff',
                      color: Object.keys(selectedPairs).includes(word) ? '#888' : '#000',
                    }}
                    onClick={() => handleWordClick(word, 1)}
                  >
                    {word}
                  </div>
                ))}
              </div>
              <div>
                {pairs[pairIndex].column2.map((word) => (
                  <div
                    key={word}
                    style={{
                      padding: '8px',
                      marginBottom: '8px',
                      border: '1px solid #ccc',
                      borderRadius: '4px',
                      cursor: Object.values(selectedPairs).includes(word) ? 'not-allowed' : 'pointer',
                      backgroundColor: Object.values(selectedPairs).includes(word) ? '#e0e0e0' : '#fff',
                      color: Object.values(selectedPairs).includes(word) ? '#888' : '#000',
                    }}
                    onClick={() => handleWordClick(word, 2)}
                  >
                    {word}
                  </div>
                ))}
              </div>
              {message && (
                <div style={{ gridColumn: 'span 2', marginTop: '16px', padding: '8px', color: '#fff', backgroundColor: '#333', borderRadius: '4px' }}>
                  {message}
                </div>
              )}
            </div>)}

            {showCourse && (<Flashcards text={courseText} words={wordViewModels} onDone={() => setShowCourse(false)} />)}
          </div>
        </div>

        {loading && <SpinnerOverlay />}
        <ModalView ref={modalRef} modalTitle={modalTitle} modalBody={modalBody} onClick={onModalClick} />
      </div>
    );
  };
