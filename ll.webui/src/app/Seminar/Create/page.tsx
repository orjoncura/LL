"use client";
import Button from 'react-bootstrap/Button';
import Alert from "react-bootstrap/Alert";
import React, {useState, useRef} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import ModalView from '@/components/Modal/ModalView';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import { POST } from '@/scripts/Helpers/SecurityHelper'
import {SeminarRequestModel, SeminarViewModel} from '@/generated-client/src';

export default function CreateSeminar() {
  
    const modalRef = useRef<any>(null); 

    const [totalCourses, setTotalCourses] = useState<number>(0);
    const [sentenceIndex, setSentenceIndex] = useState<number>(0);
    const [sentences, setSentences] = useState<string[]>([]);
    const [correctOrder, setCorrectOrder] = useState<string[]>([]);
    const [options, setOptions] = useState<string[]>([]);
    const [alertVariant, setAlertVariant] = useState("info");
    const [feedback, setFeedback] = useState("");
    const [loading, setLoading] = useState(false);
    const [modalTitle, setModalTitle] = useState('');
    const [modalBody, setModalBody] = useState('');
    const [text, setText] = useState<string>('');
    const [showSeminar, setShowSeminar] = useState(false);
    const [selectedWords, setSelectedWords] = useState<string[]>([]);
    const [onModalClick, setOnModalClick] = useState<(() => void) | undefined>(undefined);

    const openModal = (title: string, body:string, onClick?: Function) => {
      if (modalRef.current) {

          setModalTitle(title);
          setModalBody(body);
          setOnModalClick(() => onClick); 

          modalRef.current.openModal(); // Call openModal from the Example component
      }
  };

    const handleSubmit = async () => {

        try {

            if(text.length == 0)
            {
              openModal("Error", "Text can not be empty.");
              return;
            }

            const data: SeminarRequestModel = {
                "text": text,
                "languageFromId": 2,
                "languageToId": 1
            };

            setLoading(true);
            POST('/Seminar/Create', JSON.stringify(data))
            .then((SeminarViewModels: SeminarViewModel[]) => {

                    SeminarViewModels
                    .sort((a, b) => (a.importance > b.importance ? -1 : 1))
                    .forEach(m => { 

                      if(m != null && m.sentences != null){
                        m.sentences.forEach(s => { 
                          if(s.originalStatement != null)
                            sentences.push(s.originalStatement);
                        });
                      }
                    });

                    setCorrectOrder(sentences[sentenceIndex].split(" "));
                    setOptions(sentences[sentenceIndex].split(" "));
                    setTotalCourses(sentences.length);
                    setShowSeminar(true);
                    setLoading(false);
                    
                }).catch(e => {
                    setLoading(false);
                });

        } catch (error) {
            console.error('Error making API call:', error);
        }
    };

    const removeSelectedWord = (word: string) => {

      // Avoid adding if already added
      if (options.includes(word)) return;
      setOptions([...options, word]);

      const updatedSelectedWords= selectedWords.filter(
        (selectedWord) => selectedWord !== word
      );

      setSelectedWords(updatedSelectedWords);
    };

    // Handle when the user clicks on a word
    const addWord = (word: string) => {
      // Avoid adding if already added
      if (selectedWords.includes(word)) return;
      setSelectedWords([...selectedWords, word]);

      const updatedOptions = options.filter(
        (option) => option !== word
      );

      setOptions(updatedOptions);
    };
  
    // Check if the user's sentence is correct
    const checkAnswer = () => {

      if(selectedWords == correctOrder){

        setFeedback("Failed");
        setAlertVariant("success");
      }else{

        setFeedback("Success");
        setAlertVariant("danger");
      }

      setSentenceIndex(sentenceIndex + 1);
      setCorrectOrder(sentences[sentenceIndex].split(" "));
      setOptions(sentences[sentenceIndex].split(" "));
      setSelectedWords([]);
    };

    function Content() {
      return <div>
            <br />
              <textarea
                value={text}
                onChange={e => setText(e.target.value)}
                placeholder="Enter your text here..."
                rows={10}
                cols={50}
                style={{ marginBottom: '10px', width: '100%' }}
              />
              <br />
              <Button variant="success" onClick={handleSubmit}>Submit</Button>
          </div>
    }

    function SeminarContent() {
      return <div className="container mt-4">
        <div className="mb-4">
          <div className="d-flex">
            {Array.from({ length: totalCourses }).map((_, index) => (
              <div
                key={index}
                className={`flex-fill me-1 progress-bar ${
                  index < sentenceIndex ? "bg-success" : "bg-secondary"
                }`}
                style={{
                  height: "20px",
                  marginRight: index < totalCourses - 1 ? "2px" : "0",
                }}
              ></div>
            ))}
          </div>
        </div>
  
        <div className="text-center mb-4">
          <div className="p-4 bg-light border rounded shadow-sm">
              <blockquote className="quote">
                <p className="text-black">
                {sentences[sentenceIndex]}
                </p>
              </blockquote>
  
              <div>
                <div style={{ display: "flex", flexWrap: "wrap" }}>
                  {selectedWords.map((word, index) => (
                    <Button key={index} variant="success" className="me-3" onClick={() => removeSelectedWord(word)}>
                    {word}
                    </Button>
                  ))}
                </div>
              </div>
          </div>
        </div>
  
        <div>
          <div style={{ display: "flex", flexWrap: "wrap" }}>
            {options.map((word, index) => (
              <Button key={index} variant="success" className="me-3" onClick={() => addWord(word)}>
              {word}
              </Button>
            ))}
          </div>
        </div>

        <br/>
        <div className="d-flex justify-content-between align-items-center p-3 bg-light border rounded shadow-sm">
          <Alert variant={alertVariant} className="mb-0 flex-grow-1 me-3">
            {feedback}
          </Alert>
          <Button variant="success" onClick={checkAnswer}>
            Confirm
          </Button>
        </div>
      </div>
    };

    
    return (
      <div style={{ background: 'inherit' }} >
        <Navbar /> 

        <div className="container">
          {showSeminar == false && Content()}
          {showSeminar && SeminarContent()}
        </div>
        
        {loading && <SpinnerOverlay />}
        <ModalView ref={modalRef} modalTitle={modalTitle} modalBody={modalBody} onClick={onModalClick} />
      </div>
    );
  };
