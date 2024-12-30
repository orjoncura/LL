"use client";
import Button from 'react-bootstrap/Button';
import React, {useState, useEffect, useRef, CSSProperties} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import ModalView from '@/components/Modal/ModalView';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import { POST, CreateAudio } from '@/scripts/Helpers/SecurityHelper'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faVolumeUp } from '@fortawesome/free-solid-svg-icons';
import {SeminarRequestModel, SeminarViewModel, StatementShort, WordViewModel} from '@/generated-client/src';
import './page.css'; 
import { json } from 'stream/consumers';

export default function CreateSeminar() {
  
    const modalRef = useRef<any>(null); 

    const [selectedIndex, setSelectedIndex] = useState<number>(0);
    const [courseLength, setCourseLength] = useState<number>(0);
    const [wordViewModels, setWordViewModels] = useState<WordViewModel[]>([]);
    const [loading, setLoading] = useState(false);
    const [modalTitle, setModalTitle] = useState('');
    const [modalBody, setModalBody] = useState('');
    const [text, setText] = useState<string>('');
    const [showSeminar, setShowSeminar] = useState(false);
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
    
    const handleSubmit = async () => {

        try {

            if(text.length == 0)
            {
              openModal("Error", "Text can not be empty.");
              return;
            }

            let words: string[] = text.split(" ");

            setCourseLength(words.length);

            words.forEach(word => {

              setLoading(true);

              const data: SeminarRequestModel = {
                "text": word.replace(/[^a-zA-Z0-9]/g, ''),
                "languageFromId": 2,
                "languageToId": 1
              };

              POST('/Seminar/Create', JSON.stringify(data))
              .then((SeminarViewModels: SeminarViewModel) => {

                if(SeminarViewModels == null || SeminarViewModels == undefined)
                  return;

                  SeminarViewModels.words
                  .sort((a, b) => (a.importance > b.importance ? 1 : -1))
                  .forEach(m => wordViewModels.push(m));
                  
                  if(wordViewModels.length == 1){
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

    const nextStep = (newIndex: number) => {

      setShowSeminar(wordViewModels[newIndex] != null 
        && wordViewModels[newIndex].name != null)

      setSelectedIndex(newIndex < wordViewModels.length ? newIndex : 0);
    }

    return (
      <div style={{ background: 'inherit' }} >
        <Navbar /> 

        <div className="container-flex"                
          onKeyDown={(e) => {
            if (e.key === "Enter" && !e.shiftKey && "form" in e.target) {
              e.preventDefault();
              handleSubmit();
            }
          }}>

          <div className="container mt-4">
            <div className="mb-4">
              <div className="d-flex">
                {Array.from({ length: courseLength }).map((_, index) => (
                  <div
                    key={index}
                    className={`flex-fill me-1 progress-bar ${
                      index < selectedIndex ? "bg-success" : "bg-secondary"
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
                <h2 style={{ display: 'inline-block', marginRight: '10px' }}>
                  {wordViewModels[selectedIndex].name} - {wordViewModels[selectedIndex].translation}
                </h2>
                <button onClick={() => CreateAudio(wordViewModels[selectedIndex].id)} style={{ display: 'inline-block', background: 'none', border: 'none', fontWeight: 'bold', color: 'black', fontSize: '22px' }}>
                  <FontAwesomeIcon icon={faVolumeUp} />
                </button>

                {wordViewModels[selectedIndex].meanings && wordViewModels[selectedIndex].meanings.length > 0 ? (
                  wordViewModels[selectedIndex].meanings.map((meaning, index) => (
                    <div key={index} style={{ color: 'black', fontFamily: 'fangsong' }}>
                      <h3>{meaning.type}</h3>
                      <ul>
                        {meaning.definitions && meaning.definitions.length > 0 ? (
                          meaning.definitions.map((definition, defIndex) => (
                            <li key={defIndex}>{definition}</li>
                          ))
                        ) : (
                          <li>No definitions available</li>
                        )}
                      </ul>
                    </div>
                  ))
                ) : (
                  <p>No meanings available</p>
                )}
            </div>
            )}

            <br/>
          </div>
        </div>
        
        {loading && <SpinnerOverlay />}
        <ModalView ref={modalRef} modalTitle={modalTitle} modalBody={modalBody} onClick={onModalClick} />
      </div>
    );
  };
