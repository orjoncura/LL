"use client";
import Button from 'react-bootstrap/Button';
import React, {useState, useEffect, useRef, CSSProperties} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import ModalView from '@/components/Modal/ModalView';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import { GET, POST, CreateAudio } from '@/scripts/Helpers/SecurityHelper'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faVolumeUp } from '@fortawesome/free-solid-svg-icons';
import {CourseRequestModel, ExerciseRequestModel, CourseViewModel, ExerciseViewModel, WordViewModel} from '@/scripts/models';
import './page.css'; 

export default function CreateSeminar() {
    const modalRef = useRef<any>(null); 

    const [courseLength, setCourseLength] = useState<number>(0);  
    const [exerciseIndex, setExercisesIndex] = useState<number>(0);  
    const [exercises, setExercises] = useState<ExerciseViewModel[]>([]);
    const [correctOrder, setCorrectOrder] = useState<string[]>([]);
    const [wordIndex, setWordIndex] = useState<number>(0);
    const [showKeyWords, setShowKeyWords] = useState(false);
    const [keyWords, setKeyWords] = useState<WordViewModel[]>([]);
    const [pairs, setPairs] = useState<{ column1: string[]; column2: string[] }>({column1: [], column2: [],});
    const [activeWord, setActiveWord] = useState<string | null>(null);
    const [selectedPairs, setSelectedPairs] = useState<{ [key: string]: string }>({});
    const [message, setMessage] = useState<string | null>(null);
    const [wordViewModels, setWordViewModels] = useState<WordViewModel[]>([]);
    const [options, setOptions] = useState<string[]>([]);
    const [showFeedback, setShowFeedback] = useState(false);
    const [feedback, setFeedback] = useState("");
    const [loading, setLoading] = useState(false);
    const [modalTitle, setModalTitle] = useState('');
    const [modalBody, setModalBody] = useState('');
    const [text, setText] = useState<string>('');
    const [showCourse, setShowCourse] = useState(false);
    const [selectedWords, setSelectedWords] = useState<string[]>([]);  
    const [draggedIndex, setDraggedIndex] = useState<number | null>(null);
    const [onModalClick, setOnModalClick] = useState<(() => void) | undefined>(undefined);

    const [feedbackStyle, setFeedbackStyle] = useState<CSSProperties>({
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
      if (word === '' || Object.keys(selectedPairs).includes(word) || Object.values(selectedPairs).includes(word)) {
        return; // Ignore clicks on empty or already selected words
      }
  
      if (column === 1) {
        setActiveWord(word);
        setMessage(null);
      } else if (column === 2 && activeWord) {
        const index1 = pairs.column1.indexOf(activeWord);
        const index2 = pairs.column2.indexOf(word);
        const correct = index1 === index2;
  
        if (correct) {
          setSelectedPairs((prev) => ({ ...prev, [activeWord]: word }));
          setActiveWord(null);
          setMessage('Correct match!');
        } else {
          setMessage('Incorrect match! Try again.');
        }
      }
    };

    const handleSubmit = async () => {

        try {

            if(text.length == 0)
            {
              openModal("Error", "Text can not be empty.");
              return;
            }

            startCourse(text);

            const courseRequestModel: CourseRequestModel = {
              "text": text,
              "languageFromId": languageFromId,
              "languageToId": languageToId
            };

            POST('/Course/Create', JSON.stringify(courseRequestModel))
            .then((courseViewModel: CourseViewModel) => {

              if(courseViewModel == null || courseViewModel == undefined)
                return;

              courseViewModel.words
                .sort((a, b) => (a.importance > b.importance ? 1 : -1))
                .forEach(w => {

                  const createDefinitionsModel: CourseRequestModel = {
                    "text": w.word,
                    "languageFromId": languageFromId,
                    "languageToId": languageToId
                  };

                  POST('/Course/CreateDefinitions', JSON.stringify(createDefinitionsModel))
                  .then((wordViewModel: WordViewModel) => {
  
                    startCourse(text);

                    if(wordViewModel == null || wordViewModel == undefined || wordViewModel.id == 0)
                      return;
  
                    wordViewModels.push(wordViewModel)

                    const exerciseRequestModel: ExerciseRequestModel = {
                      "courseId": courseViewModel.id,
                      "text": courseRequestModel.text,
                      "languageFromId": courseRequestModel.languageFromId,
                      "languageToId": courseRequestModel.languageToId,
    
                      "wordId": wordViewModel.id,
                      "wordName": wordViewModel.name,
                      "rankId": w.importance,
                    };
    
                    POST('/Course/CreateExercises', JSON.stringify(exerciseRequestModel))
                    .then((exerciseViewModels: ExerciseViewModel[]) => {
    
                      if(exerciseViewModels == null || exerciseViewModels.length == 0)
                        return;
    
                      exerciseViewModels.forEach(e => exercises.push(e));
  
                      if(exerciseIndex == 0)
                      {
                        setLoading(false);
                        nextStep(exerciseIndex);
                      }                     
  
                    })});

                  })});

        } catch (error) {
            console.error('Error making API call:', error);
        }
    };

    const startCourse = (text: string) => {

      if(showCourse == false && showKeyWords == false){

        let words: WordViewModel[] = keyWords.filter(w => text.split(" ").includes(w.name));

        if(words.length > 0)
          setPairs({column1: words.map(w => w.name), column2: words.map(w => w.translation || "")});
        
        setLoading(keyWords.length == 0 && exercises.length == 0);
        setShowKeyWords(keyWords.length > 0);
        setShowCourse(exercises.length > 0);
      }
    }

    const nextStep = (newIndex: number) => {

      let wordViewModel: WordViewModel = wordViewModels[wordIndex];
      let exercise: ExerciseViewModel = exercises[newIndex];

      setShowCourse(wordViewModel != null 
        && wordViewModel.name != null
        && exercise != null
        && showKeyWords == false)
      
      if(wordViewModels[wordIndex] == null){

        setWordViewModels([]);
        setCourseLength(0);
      }

      setWordIndex(newIndex < wordViewModels.length ? newIndex : 0);
      setExercisesIndex(newIndex < exercises.length ? newIndex : 0);

      let exerciseInCorrectOrder: string[] = [];
      let options: string[] = [];

      if(exercise == null){

        setExercises([]);
        setCourseLength(0);
        
      }else{

        if(exercise.translated != null)
          exerciseInCorrectOrder = exercise.translated.split(" ").map(w => w.replace(/[^a-zA-Z0-9]/g, ''));

        if(exercise.extra != null)
          options = exerciseInCorrectOrder.concat(exercise.extra.split(" ").map(w => w.replace(/[^a-zA-Z0-9]/g, '')));
      }
      
      setCourseLength(wordViewModels.length * 3);
      setCorrectOrder(exerciseInCorrectOrder);
      setOptions(shuffle(options));    
      setSelectedWords([]);
      setShowFeedback(false);
      setExercises(exercises);
    }

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

      if(showCourse == false && showKeyWords == false)
        return handleSubmit();

      if(showKeyWords == true){
       setShowCourse(pairs.column1.length == Object.keys(selectedPairs).length)
       setShowKeyWords(pairs.column1.length != Object.keys(selectedPairs).length)
       return;
      }

      if(areArraysEqual(selectedWords, correctOrder)){

        setFeedbackStyle({color: "#0F766E",backgroundColor: "#F0FDFA"});
        setFeedback("That's Correct");

      }else{
        
        setFeedbackStyle({color: "#d63384",backgroundColor: "#fff0f6"});
        setFeedback("That's incorrect");
      }

      setShowFeedback(true);
    };

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

          <div className="container mt-4" style={{ marginBottom: "25%" }}>
            <div className="mb-4">
              <div className="d-flex">
                {Array.from({ length: courseLength }).map((_, index) => (
                  <div
                    key={index}
                    className={`flex-fill me-1 progress-bar ${
                      index < exerciseIndex ? "bg-success" : "bg-secondary"
                    }`}
                    style={{
                      height: "20px",
                      marginRight: index < courseLength - 1 ? "2px" : "0",
                    }}
                  ></div>
                ))}
              </div>
            </div>

            {showCourse == false && showKeyWords == false && ( 
              <div>
                <div style={{textAlign: 'center'}}>
                  <b>Transform your ideas into a unique and impactful learning experience</b>
                  <label>We empower you to leverage provided input to create a customized educational journey that aligns perfectly
                        with your specific goals and needs.</label>
                </div>

                <br/><br/><br/>
                <textarea
                        value={text}
                        onChange={e => setText(e.target.value)}
                        placeholder="Please enter the text you would like to translate..."
                        rows={10}
                        cols={50}
                        style={{ marginBottom: '10px', width: '100%' }}
                      />
              </div>
            )}

            {showKeyWords && (
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px', maxWidth: '400px', margin: '0 auto', textAlign: 'center', padding: '16px' }}>
              <div style={{ gridColumn: 'span 2', marginBottom: '16px' }}>
                <h1 style={{ fontSize: '24px', fontWeight: 'bold' }}>Match the Words</h1>
                <p style={{ fontSize: '16px', color: '#555' }}>Select the matching pairs from the two columns below.</p>
              </div>
              <div>
                {pairs.column1.map((word) => (
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
                {pairs.column2.map((word) => (
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

            {showCourse && (   
              <div>
                <h2 style={{ display: 'inline-block', marginRight: '10px' }}>
                  {wordViewModels[wordIndex].name} - {wordViewModels[wordIndex].translation}
                </h2>
                <button onClick={() => CreateAudio(wordViewModels[wordIndex].id)} className='audio'>
                  <FontAwesomeIcon icon={faVolumeUp} />
                </button>

                {wordViewModels[wordIndex].meanings && wordViewModels[wordIndex].meanings.length > 0 ? (
                  wordViewModels[wordIndex].meanings.map((meaning, index) => (
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
              <br/>

              {showCourse && exercises.length > 0 && exercises[exerciseIndex] != null && (
                <div>
                  <div className="text-center mb-4">
                    <div className="p-4 bg-light border rounded shadow-sm">
                        <blockquote className="quote">
                          <p className="text-black">
                          {exercises[exerciseIndex].original}
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
                </div>)}
          </div>
        </div>
        
        <div className="feedback-container fixed-bottom" style={feedbackStyle}>
          {showFeedback &&  (
            <div className="feedback-text">
              <div className="feedback-details">
                <h3 className="feedback-title">{feedback}</h3>
                {exercises[exerciseIndex] != null && (<h4 className="feedback-detail">Answer: {exercises[exerciseIndex].translated}</h4>)}
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
            <button className="feedback-button" onClick={() => nextStep(exerciseIndex + 1)}>
              <span className="chevron">›</span>
                Next
            </button>
          )}
        </div>

        {loading && <SpinnerOverlay />}
        <ModalView ref={modalRef} modalTitle={modalTitle} modalBody={modalBody} onClick={onModalClick} />
      </div>
    );
  };
