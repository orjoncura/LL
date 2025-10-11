"use client";
import React, { useRef, useState, useEffect, ChangeEvent, KeyboardEvent } from 'react';
import Navbar from '@/components/Navbar/Navbar';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import FeedbackView from '@/components/Feedback/FeedbackView';
import { GET, POST } from '@/utils/Security/httpClient'
import {CourseRequestModel, WordViewModel} from '@/utils/Models/models';
import { SendLocalNotifications } from '@/utils/System/Notification'
import { useToast } from '@/components/Toast/Toast'; 
import '@/components/Feedback/FeedbackView.css'; 
import './page.css'; 

const CreateCourse: React.FC = () => {
  const fileInputRef = useRef<HTMLInputElement | null>(null);
  const [showKeyWords, setShowKeyWords] = useState(false);
  const [keyWords, setKeyWords] = useState<WordViewModel[]>([]);
  const [wordViewModels, setWordViewModels] = useState<WordViewModel[]>([]);    
  const feedbackViewRef = useRef<any>(null); 
  const [loading, setLoading] = useState(false);
  const [fbTitle, setfbhTitle] = useState('');
  const [fbBody, setfbhBody] = useState('');
  const [showCourse, setShowCourse] = useState(false);
  const [url, setURL] = useState<string>('');   
  const [courseText, setCourseText] = useState<string>('');   
  const { showToast, ToastContainer } = useToast();

  const showFeedback = (title: string, body:string, onClick?: Function) => {
      if (feedbackViewRef.current) {

          setfbhTitle(title);
          setfbhBody(body);

          feedbackViewRef.current.open();
      }
  };

  const handleFileSelect = (e: ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file && file.type === 'text/plain') {
      const reader = new FileReader();
      reader.onload = (event: ProgressEvent<FileReader>) => {
        const fileText = event.target?.result as string;
        setCourseText(fileText);
      };
      reader.readAsText(file);
    } else {
      showFeedback("Error", 'Please select a valid .txt file');
    }
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

            if(courseText.length == 0 && url.length == 0)
            {
              showFeedback("Error", "Text can not be empty.");
              return;
            }

            setLoading(true);

            if(courseText == "" && url.length > 1){
              const responseText = await GET('/Speech/SpeechToText?url=' + encodeURIComponent(url));
              setCourseText(responseText);
            }

            var textCleaned = courseText.replaceAll(/[\r\n]+/g, " ");
            startCourse(textCleaned);

            const oembedUrl = `https://www.youtube.com/oembed?url=${encodeURIComponent(url)}&format=json`;
            fetch(oembedUrl)
            .then((res) => {
              
              if (!res.ok) {
                showFeedback("Error", "Failed to fetch video info.");
                setLoading(false);
                return;
              }

              return res.json()
            })
            .then((data: any) => {

              if(courseText.length == 0) {
                showFeedback("Error", "Please try again later.");
                setLoading(false);
                return;
              }

              const courseRequestModel: CourseRequestModel = {
                "title": data.title,
                "text": courseText,
                "languageFromId": languageFromId,
                "languageToId": languageToId
              };

              POST('/Course/Create', JSON.stringify(courseRequestModel))
              .then((isSuccessful: boolean) => {
        
                if(isSuccessful)
                  SendLocalNotifications("Your new lesson is ready!!", "Please go to courses to start learning!");
        
              });

              showToast("Something exciting is coming… a brand new course for " + data.title + " is on the way!");
            })
            .catch((err) => {
                showFeedback("Error", err.message);
                setLoading(false);
                return;
            });

            setCourseText("");
            setLoading(false);
          
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
      
    function getYouTubeEmbedUrl() : string {
      try {
        const urlObj = new URL(courseText);

        // Case 1: normal YouTube link
        if (urlObj.hostname.includes("youtube.com")) {
          return `https://www.youtube.com/embed/${urlObj.searchParams.get("v")}`;
        }

        // Case 2: short link (youtu.be)
        if (urlObj.hostname.includes("youtu.be")) {
          return `https://www.youtube.com/embed${urlObj.pathname}`;
        }

        return ""; // not a YouTube URL
      } catch {
        return "";
      }
    }

  const handleKeyDown = (e: KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      e.preventDefault();
      handleSubmit();
    }
  };

  return (
    <div>

      <Navbar /> 

      <div className="center-container">
        <div  style={{textAlign: 'center'}}>
           <div >
              <b className='mainTxt'>Transform your ideas into a unique and impactful learning experience</b>
              <br></br>
              <label>We empower you to leverage provided input to create a customized educational journey that aligns perfectly
                    with your specific goals and needs.</label>
           </div> 
          <br></br>
          <div className="search-bar glow-frame">
            <button
              type="button"
              className="search-button plus-button"
              onClick={() => fileInputRef.current?.click()}
              title="Upload text file"
            >
              +
            </button>

            <input
              ref={fileInputRef}
              type="file"
              accept=".txt"
              className="hidden-file-input mainTxt glow-frame"
              onChange={handleFileSelect}
            />

            <input
              type="text"
              value={url}
              onChange={e => setURL(e.target.value)}
              onKeyDown={handleKeyDown}
              placeholder="www.youtube.com/watch?v=example"
              className="search-input mainTxt"
            />

            <button
              type="button"
              className="search-button submit-button"
              onClick={handleSubmit}
              title="Submit"
            >
              ↑
            </button>
          </div>

          {getYouTubeEmbedUrl() &&
            <div className="flex justify-center p-4" style={{display: 'flex', justifyContent: 'center'}}>
                <iframe
                  className="rounded-2xl shadow-lg"
                  width="560"
                  height="315"
                  src={getYouTubeEmbedUrl()}
                  title="YouTube video preview"
                allowFullScreen
              ></iframe>
            </div> }
        </div>
      </div>

        <ToastContainer />
        {loading && <SpinnerOverlay />}
        <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody}/>

    </div>
  );
};

export default CreateCourse;
