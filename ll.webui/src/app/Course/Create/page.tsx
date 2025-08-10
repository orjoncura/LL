"use client";

import React, {useState, useRef, useEffect} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import Flashcards from '@/components/Courses/Flashcards';
import Multiselect from '@/components/Courses/Multiselect';
import FeedbackView from '@/components/Feedback/FeedbackView';
import { GET, POST, CreateAudio } from '@/utils/Security/httpClient'
import {CourseRequestModel, WordViewModel} from '@/utils/Models/models';
import { SendLocalNotifications } from '@/utils/System/Notification'

import './page.css'; 

export default function CreateCourse() {

    const [showKeyWords, setShowKeyWords] = useState(false);
    const [keyWords, setKeyWords] = useState<WordViewModel[]>([]);
    const [wordViewModels, setWordViewModels] = useState<WordViewModel[]>([]);    
    const [loading, setLoading] = useState(false);
    const feedbackViewRef = useRef<any>(null); 
    const [fbTitle, setfbhTitle] = useState('');
    const [fbBody, setfbhBody] = useState('');
    const [showCourse, setShowCourse] = useState(false);
    const [courseText, setCourseText] = useState<string>('');   

    const showFeedback = (title: string, body:string) => {
            setfbhTitle(title);
            setfbhBody(body);
    };

    const languageFromId = 2;
    const languageToId = 1;

    useEffect(() => {

      if(showKeyWords == false && loading == true && wordViewModels.length > 0){

        setLoading(false);
        setShowCourse(true)
      }
    }, [loading, showKeyWords, wordViewModels]);

    let hasFetchedData = false;
    useEffect(() => {

      if(hasFetchedData == false){
        hasFetchedData = true;

        GET('/Course/GetKeyWords?languageId=' + languageFromId)
        .then((words: WordViewModel[]) => {
  
          if(words == null || words == undefined)
            return;
  
          setKeyWords(words.filter(w => w.importanceRatingId == 1));
        });
      }
    }, []);

    const handleSubmit = async () => {

        try {

            setLoading(true);
            if(courseText.length == 0)
            {
              showFeedback("Error", "Text can not be empty.");
              return;
            }

            var textCleaned = courseText.replaceAll(/[\r\n]+/g, " ");
            startCourse(textCleaned);

            const courseRequestModel: CourseRequestModel = {
              "text": textCleaned,
              "languageFromId": languageFromId,
              "languageToId": languageToId
            };

            POST('/Course/Create', JSON.stringify(courseRequestModel))
            .then((isSuccessful: boolean) => {
      
              if(isSuccessful)
                SendLocalNotifications("Your new lesson is ready!!", "Please go to courses to start learning!");
      
            }).finally(() => setLoading(false));
          
          } catch (error) {

            setLoading(false);
            console.error('Error making API call:', error);
        }
    };

    const startCourse = (text: string) => {

      if(showCourse || showKeyWords)
        return;

      let wordList = text.split(" ");
      let kw: WordViewModel[] = keyWords.filter(w => wordList.includes(w.name) && w.importanceRatingId == 1);


      if(wordViewModels.length == 0){

          const words = keyWords.filter(w => wordList.includes(w.name) && w.importanceRatingId == 2);
          setWordViewModels(prev => [...prev, ...words]);
      }

      setShowKeyWords(kw.length > 0);
      setLoading(kw.length == 0);
    }
      
    return (
      <div>

        <Navbar /> 

        <div className="container-flex">
          <div className="container mt-4" style={{ marginBottom: "25%" }}>
  
            {showCourse == false && showKeyWords == false && ( 
            <div className='glow-frame '>
                <div  style={{textAlign: 'center'}}>
                  <b className='mainTxt'>Transform your ideas into a unique and impactful learning experience</b>
                  <label>We empower you to leverage provided input to create a customized educational journey that aligns perfectly
                        with your specific goals and needs.</label>
                </div> 

            <div className="mirror-textarea-container">
                <div className="mirror-text">
                  {courseText}
                </div>

                <textarea 
                  id='txtBar'
                  value={courseText}
                  placeholder='Enter your new idea here'
                  onChange={e => setCourseText(e.target.value)}
                  onKeyDown={(e) => {
                    if (e.key === 'Enter') {
                      e.preventDefault();
                        handleSubmit();
                    }}}

                  className='mainTxt auto-textarea'
                /> 
            </div>

            <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody} text={"Confirm"} onClick={handleSubmit} isVisible={true} showCloseBtn={false} />
            </div>
            )}

            {showKeyWords && (<Multiselect words={keyWords} onDone={() => {setShowKeyWords(false); setShowCourse(wordViewModels[0] != null); setLoading(false); }}/>)}
            {showCourse && (<Flashcards text={courseText} words={wordViewModels} onDone={() => setShowCourse(false)}  />)}
          </div>
        </div>

        {loading && <SpinnerOverlay />}
        
      </div>
    );
  };
