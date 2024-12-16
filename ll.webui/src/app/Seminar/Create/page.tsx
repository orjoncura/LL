"use client";
import Button from 'react-bootstrap/Button';
import React, {useState, useEffect, useRef, CSSProperties} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import ModalView from '@/components/Modal/ModalView';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import { POST } from '@/scripts/Helpers/SecurityHelper'
import {SeminarRequestModel, SeminarViewModel, StatementShort} from '@/generated-client/src';
import './page.css'; 

export default function CreateSeminar() {
  
    const modalRef = useRef<any>(null); 

    const [sentenceIndex, setSentenceIndex] = useState<number>(0);
    const [courseLength, setCourseLength] = useState<number>(0);
    const [sentences, setSentences] = useState<StatementShort[]>([]);
    const [correctOrder, setCorrectOrder] = useState<string[]>([]);
    const [options, setOptions] = useState<string[]>([]);
    const [showFeedback, setShowFeedback] = useState(false);
    const [feedback, setFeedback] = useState("");
    const [loading, setLoading] = useState(false);
    const [modalTitle, setModalTitle] = useState('');
    const [modalBody, setModalBody] = useState('');
    const [text, setText] = useState<string>('');
    const [showSeminar, setShowSeminar] = useState(false);
    const [selectedWords, setSelectedWords] = useState<string[]>([]);  
    const [draggedIndex, setDraggedIndex] = useState<number | null>(null);
    const [onModalClick, setOnModalClick] = useState<(() => void) | undefined>(undefined);

    const [feedbackStyle, setFeedbackStyle] = useState<CSSProperties>({
      color: "#d63384",
      backgroundColor: "#fff0f6"
    });

    const openModal = (title: string, body:string, onClick?: Function) => {
      if (modalRef.current) {

          setModalTitle(title);
          setModalBody(body);
          setOnModalClick(() => onClick); 

          modalRef.current.openModal(); // Call openModal from the Example component
      }
    };

    function shuffle<T>(array: string[]): string[] {
  
      const shuffled = [...array];

      for (let i = shuffled.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [shuffled[i], shuffled[j]] = [shuffled[j], shuffled[i]];
      }

      return shuffled;
    }
    
    function areArraysEqual(arr1: string[], arr2: string[]): boolean {
      
      if (arr1.length !== arr2.length) return false;
  
      for (let i = 0; i < arr1.length; i++) {
          if (arr1[i] !== arr2[i]) {
              return false;
          }
      }
  
      return true;
    }

    const handleSubmit = async () => {

        try {

            if(text.length == 0)
            {
              openModal("Error", "Text can not be empty.");
              return;
            }

            let words: string[] = text.split(" ");

            setCourseLength(words.length * 3);

            words.forEach(word => {

              setLoading(true);

              const data: SeminarRequestModel = {
                "text": word.replace(/[^a-zA-Z0-9]/g, ''),
                "languageFromId": 2,
                "languageToId": 1
              };

              POST('/Seminar/Create', JSON.stringify(data))
              .then((SeminarViewModels: SeminarViewModel[]) => {

                  SeminarViewModels
                  .sort((a, b) => (a.importance > b.importance ? 1 : -1))
                  .forEach(m => { 
                    if(m != null && m.sentences != null){

                      m.sentences.forEach(s => { 
                          sentences.push(s);
                      });
                      
                    }
                  });
                  
                  if((sentences.length / 3) == 1){
                    nextStep(0);
                    setLoading(false);
                  }

                }).catch(e => {
                    setLoading(false);
                });
            });

        } catch (error) {
            console.error('Error making API call:', error);
        }
    };

    const onDragStart = (index: number) => {
      setDraggedIndex(index);
    };
  
    const onDragOver = (event: React.DragEvent<HTMLButtonElement>) => {
      event.preventDefault(); // Necessary to allow dropping
    };
  
    const onDrop = (index: number) => {
      if (draggedIndex === null) return;
  
      const newItems = [...selectedWords];
      const [draggedItem] = newItems.splice(draggedIndex, 1); // Remove dragged item
      newItems.splice(index, 0, draggedItem); // Insert at drop position
  
      setSelectedWords(newItems);
      setDraggedIndex(null); // Reset dragged index
      }

    const removeWord = (word: string) => {

      setOptions([...options, word]);

      const updatedSelectedWords = [...selectedWords];
      const index = selectedWords.indexOf(word);
      if (index > -1) {
        updatedSelectedWords.splice(index, 1);
      }
    
      setSelectedWords(updatedSelectedWords);
    };

    // Handle when the user clicks on a word
    const addWord = (word: string) => {

      setSelectedWords([...selectedWords, word]);

      const updatedOptions = [...options];

      const index = updatedOptions.indexOf(word);
      if (index > -1) {
        updatedOptions.splice(index, 1);
      }
    
      setOptions(updatedOptions);
    };
  
    // Check if the user's sentence is correct
    const checkAnswer = () => {

      if(showSeminar == false)
        return handleSubmit();

      if(areArraysEqual(selectedWords, correctOrder)){

        setFeedbackStyle({color: "#0F766E",backgroundColor: "#F0FDFA"});
        setFeedback("That's Correct");

      }else{
        
        setFeedbackStyle({color: "#d63384",backgroundColor: "#fff0f6"});
        setFeedback("That's incorrect");
      }

      setShowFeedback(true);
    };

    const nextStep = (newSentenceIndex: number) => {

      let statement: string[] = [];

      setShowSeminar(sentences[newSentenceIndex] != null 
        && sentences[newSentenceIndex].originalStatement != null 
        && sentences[newSentenceIndex].translatedStatement != null)

      setSentenceIndex(newSentenceIndex < sentences.length ? newSentenceIndex : 0);

      if(sentences[newSentenceIndex] != null && sentences[newSentenceIndex].translatedStatement != null)
        statement = sentences[newSentenceIndex].translatedStatement.split(" ").map(w => w.replace(/[^a-zA-Z0-9]/g, ''));
        
      if(sentences[newSentenceIndex] == null){

        setSentences([]);
        setCourseLength(0);
      }
      setCorrectOrder(statement);
      setOptions(shuffle(statement));    
      setSelectedWords([]);
      setShowFeedback(false);
    }

    return (
      <div style={{ background: 'inherit' }} >
        <Navbar /> 

        <div className="container-flex"                
          onKeyDown={(e) => {
            if (e.key === "Enter" && !e.shiftKey && "form" in e.target) {
              e.preventDefault();
              checkAnswer();
            }
          }}>

          <div className="container mt-4">
            <div className="mb-4">
              <div className="d-flex">
                {Array.from({ length: courseLength }).map((_, index) => (
                  <div
                    key={index}
                    className={`flex-fill me-1 progress-bar ${
                      index < sentenceIndex ? "bg-success" : "bg-secondary"
                    }`}
                    style={{
                      height: "20px",
                      marginRight: index < courseLength - 1 ? "2px" : "0",
                    }}
                  ></div>
                ))}
              </div>
            </div>
      
            {showSeminar == false && ( 
              <textarea
                      value={text}
                      onChange={e => setText(e.target.value)}
                      placeholder="Please enter the text you would like to translate..."
                      rows={10}
                      cols={50}
                      style={{ marginBottom: '10px', width: '100%' }}
                    />
            )}

            {showSeminar && (     
            <div>  
              <div className="text-center mb-4">
                <div className="p-4 bg-light border rounded shadow-sm">
                    <blockquote className="quote">
                      <p className="text-black">
                      {sentences[sentenceIndex].originalStatement}
                      </p>
                    </blockquote>
        
                    <div>
                      <div>

                        {selectedWords.map((word, index) => (
                            <Button
                                 key={index}
                                 draggable
                                 onDragStart={() => onDragStart(index)}
                                 onDragOver={onDragOver}
                                 onDrop={() => onDrop(index)}
                                 onClick={() => removeWord(word)}
                                 className="custom-button" 
                               >
                                 {word}
                          </Button>
                        ))}

                      </div>
                    </div>
                </div>
              </div>
        
              <div>
                <div style={{ display: "flex", flexWrap: "wrap" }}>

                  {options.map((word) => (
                      <Button onClick={() => addWord(word)} className="custom-button"  >
                        {word}
                      </Button>
                   ))}

                </div>
              </div>
            </div>
            )}

            <br/>
          </div>

          <div className="feedback-container fixed-bottom" style={feedbackStyle}>

            {showFeedback &&  (
              <div className="feedback-text">
                <div className="feedback-details">
                  <h3 className="feedback-title">{feedback}</h3>
                  <h4 className="feedback-detail">Answer: {sentences[sentenceIndex].translatedStatement}</h4>
                </div>
              </div>
            )}

            {showFeedback == false && (
              <button className="feedback-button" onClick={checkAnswer}>
                <span className="chevron">›</span>
                Confirm
              </button>
            )}

            {showFeedback && (
              <button className="feedback-button" onClick={() => nextStep(sentenceIndex + 1)}>
                <span className="chevron">›</span>
                  Next
              </button>
            )}
          </div>
        </div>
        
        {loading && <SpinnerOverlay />}
        <ModalView ref={modalRef} modalTitle={modalTitle} modalBody={modalBody} onClick={onModalClick} />
      </div>
    );
  };
