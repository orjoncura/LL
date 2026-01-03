"use client";

import Button from 'react-bootstrap/Button';
import React, {useState, useEffect, useRef, CSSProperties} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import { GetExercisesByModuleId, MarkModuleAsComplete } from '@/utils/Controllers/CourseController'

import {ExerciseViewModel} from '@/utils/Models/models';
import { useRouter, useParams } from 'next/navigation';

import './page.css'; 

export default function Exercises() {
  
    const [exerciseIndex, setExercisesIndex] = useState<number>(0);  
    const [exercises, setExercises] = useState<ExerciseViewModel[]>([]);
    const [correctOrder, setCorrectOrder] = useState<string[]>([]);
    const [options, setOptions] = useState<string[]>([]);
    const [showFeedback, setShowFeedback] = useState(false);
    const [feedback, setFeedback] = useState("");
    const [loading, setLoading] = useState(false);
    const [selectedWords, setSelectedWords] = useState<string[]>([]);  
    const [draggedIndex, setDraggedIndex] = useState<number | null>(null);
    const params = useParams<{ id: string; }>()
    const router = useRouter();

    const [feedbackStyle, setFeedbackStyle] = useState<CSSProperties>({
      color: "#d63384",
      backgroundColor: "#fff0f6"
    });

    let hasFetchedData = false;

    useEffect(() => {

      if(hasFetchedData == false){
        hasFetchedData = true;

        setLoading(true);

        GetExercisesByModuleId(params.id)
        .then((exerciseViewModels: ExerciseViewModel[]) => {

          if(exerciseViewModels == null || exerciseViewModels.length == 0)
            return;

          exerciseViewModels.forEach(e => exercises.push(e));

          if(exerciseIndex == 0)
            nextStep(exerciseIndex);

        }).finally(() => setLoading(false));
      }
    }, []);
    
    const nextStep = (newIndex: number) => {

      let exercise: ExerciseViewModel = exercises[newIndex];
      setExercisesIndex(newIndex < exercises.length ? newIndex : 0);

      let exerciseInCorrectOrder: string[] = [];
      let options: string[] = [];

      if(newIndex >= exercises.length){
        MarkModuleAsComplete(params.id)
        .finally(() => {router.back();});
      }

      if(exercise == null){

        router.back();
        
      }else{

        if(exercise.translated != null)
          exerciseInCorrectOrder = exercise.translated.split(" ").map(w => w.replace(/[^a-zA-Z0-9]/g, ''));

        if(exercise.extra != null)
          options = exerciseInCorrectOrder.concat(exercise.extra.split(" ").map(w => w.replace(/[^a-zA-Z0-9]/g, '')));
      }
      
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

        <div className="container-flex">
          <div className="container mt-4" style={{ marginBottom: "25%" }}>
              {exercises.length > 0 && exercises[exerciseIndex] != null && (
                <div>
                  <div className="text-center mb-4">
                    <div className="p-4 glow-frame">
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

                      {options.map((word, index) => (
                          <Button key={index} onClick={() => addWord(word)} className="custom-button"  >
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
                <h4 className="feedback-detail">Answer: {exercises[exerciseIndex].translated}</h4> 
              </div>
            </div>
          )}

          {showFeedback == false && (
            <button className="feedback-button mainBtn" onClick={checkAnswer}>
              <span className="chevron">›</span>
              Confirm
            </button>
          )}

          {showFeedback && (
            <button className="feedback-button mainBtn" onClick={() => nextStep(exerciseIndex + 1)}>
              <span className="chevron">›</span>
                Next
            </button>
          )}
        </div>

        {loading && <SpinnerOverlay />}
      </div>
    );
  };