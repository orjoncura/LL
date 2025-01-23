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
    const [keyWordsIndex, setKeyWordsIndex] = useState<number>(0);
    const [keyWords, setKeyWords] = useState<WordViewModel[]>([]);
    const [keyWordsFiltered, setKeyWordsFiltered] = useState<string[]>([]);
    const [keyWordsTranslations, setKeyWordsTranslations] = useState<string[]>([]);
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
                        nextStep(exerciseIndex);
                      }                     
  
                    })});

                  })});

        } catch (error) {
            console.error('Error making API call:', error);
        }
    };

    const startCourse = (text: string) => {

      let words: WordViewModel[] = keyWords.filter(w => text.split(" ").includes(w.name));

      setKeyWordsFiltered(words.map(w => w.name));
      setKeyWordsTranslations(words.map(w => w.translation || ""));

      const loadingTimeout:Function = async () => {

        setLoading(keyWords.length == 0);
        setShowKeyWords(keyWords.length > 0);
      }

      setTimeout(loadingTimeout, 100)
    }

    const handleWordClick = (translation: string) => {
      setShowFeedback(true);

      if (keyWordsTranslations[keyWordsIndex] == translation) {
        setFeedbackStyle({color: "#0F766E",backgroundColor: "#F0FDFA"});
        setFeedback("Correct! 🎉");
        setKeyWordsIndex(keyWordsIndex + 1);
      } else {
        setFeedbackStyle({color: "#d63384",backgroundColor: "#fff0f6"});
        setFeedback("Incorrect. Try again! ❌");
      }
    };

    const nextStep = (newIndex: number) => {

      let wordViewModel: WordViewModel = wordViewModels[wordIndex];
      let exercise: ExerciseViewModel = exercises[newIndex];

      setShowCourse(wordViewModel != null 
        && wordViewModel.name != null
        && exercise != null)

      setShowKeyWords(false);
      
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

      if(showCourse == false)
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
            <div style={{ textAlign: "center" }}>
              <h3>Match the Spanish word with its translation</h3>
              <div style={{ display: "flex", justifyContent: "center", gap: "2rem", marginTop: "2rem" }}>
                <div>
                  <h4>Spanish Words</h4>
                  {keyWordsFiltered.map((word, index) => (
                    <button
                      key={word}
                      //onClick={() => handleEnglishClick(word)}
                      style={{
                        display: "block",
                        margin: "0.5rem",
                        padding: "0.5rem 1rem",
                        border: "1px solid #ccc",
                        borderRadius: "20px", // Rounded corners
                        cursor: "pointer",
                        fontSize: "16px",
                        boxShadow: "0 2px 4px rgba(0, 0, 0, 0.1)",
                        color: "#d63384",
                        backgroundColor: keyWordsIndex <= index? "#fff0f6" : "grey",
                      }}
                        >
                          {word}
                        </button>
                      ))}
                  </div>

                <div>
                  <h4>Translations</h4>
                  {keyWordsTranslations.map((translation) => (
                    <button
                      key={translation}
                      onClick={() => handleWordClick(translation)}
                      style={{
                        display: "block",
                        margin: "0.5rem",
                        padding: "0.5rem 1rem",
                        border: "1px solid #ccc",
                        borderRadius: "20px", // Rounded corners
                        cursor: "pointer",
                        fontSize: "16px",
                        boxShadow: "0 2px 4px rgba(0, 0, 0, 0.1)",
                        color: "#d63384",
                        backgroundColor: "#fff0f6"
                      }}
                    >
                      {translation}
                    </button>
                  ))}
                </div>
              </div>
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
                {/* <h4 className="feedback-detail">Answer: {exercises[exerciseIndex].translated}</h4> */}

                
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
