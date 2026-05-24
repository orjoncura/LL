"use client";

import React, { useRef, useState, useEffect, ChangeEvent, KeyboardEvent } from 'react';
import { Plus, Send } from 'lucide-react';
import Navbar from '@/components/Navbar/Navbar';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import FeedbackView from '@/components/Feedback/FeedbackView';

import { CourseRequestModel, WordViewModel, VideoInfo } from '@/utils/Models/models';
import { LanguageEnum } from '@/utils/Models/Enums';
import { SendLocalNotifications } from '@/utils/System/Notification'
import { useToast } from '@/components/Toast/Toast'; 
import { GetKeyWords, Create } from '@/utils/Controllers/CourseController'

import '@/components/Feedback/FeedbackView.css'; 
import './page.css'; 

interface VideoInfo {
  title: string;
  author_name?: string;
  thumbnail_url?: string;
}

export default function CreateCourse() {
  const fileInputRef = useRef<HTMLInputElement | null>(null);
  const [keyWords, setKeyWords] = useState<WordViewModel[]>([]);
  const feedbackViewRef = useRef<any>(null); 
  const [loading, setLoading] = useState(false);
  const [fbTitle, setfbhTitle] = useState('');
  const [fbBody, setfbhBody] = useState('');
  const [url, setURL] = useState<string>('');   
  const [courseTitle, setCourseTitle] = useState<string>('');   
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
        setURL(file.name);
        setCourseTitle(file.name);
      };
      reader.readAsText(file);
    } else {
      showFeedback("Error", 'Please select a valid .txt file');
    }

    e.target.value = "";
  };

  let hasFetchedData = false;
  
  useEffect(() => {

    if(hasFetchedData == false){
      hasFetchedData = true;

      GetKeyWords(LanguageEnum.Spanish)
      .then((words: WordViewModel[]) => {

        if(words == null || words == undefined)
          return;

        setKeyWords(words.filter(w => w.importanceRatingId == 1));
      });
    }
  }, []);

  const handleSubmit = async () => {
      setLoading(true);

      try {
          if (courseText.length === 0 && url.length === 0) 
              throw new Error("Text can not be empty.");
          
          if (courseText.length === 0 && url.length > 0) {

            const response = await fetch(`https://www.youtube.com/oembed?url=${encodeURIComponent(url)}&format=json`);

            // Check the status of the response
            if (!response.ok) {
                throw new Error(`Failed to fetch video info: ${response.status} ${response.statusText}`);
            }

            const data: VideoInfo = await response.json();
            setCourseTitle(data.title);
          }

          console.log('courseTitle');
          console.log(courseTitle);
          const courseRequestModel: CourseRequestModel = {
              title: courseTitle,
              url: url,
              text: courseText,
              languageFromId:  LanguageEnum.Spanish,
              languageToId:  LanguageEnum.English
          };

          if (!courseRequestModel.title || !courseRequestModel.languageFromId || !courseRequestModel.languageToId) 
              throw new Error("Missing required field in course creation request");

          Create(courseRequestModel)
          .then((isSuccessful: boolean) => {
    
            if(isSuccessful)
              SendLocalNotifications("Your new lesson is ready!!", "Please go to courses to start learning!");
    
          });

          showToast("Something exciting is coming… a brand new course for " + courseTitle + " is on the way!");

          setCourseText("");
          setURL("");

          //let wordList = text.split(" ");
          //let kw: WordViewModel[] = keyWords.filter(w => wordList.includes(w.name) && w.importanceRatingId == 1);

      } catch (error: any) {  
          showFeedback("Error", error.message.substring(0, 100));
      }

      setLoading(false);
  };

  const getYouTubeEmbedUrl = (): string => {
    try {
      const urlObj = new URL(url);

      if (urlObj.hostname.includes("youtube.com")) {
        return `https://www.youtube.com/embed/${urlObj.searchParams.get("v")}`;
      }

      if (urlObj.hostname.includes("youtu.be")) {
        return `https://www.youtube.com/embed${urlObj.pathname}`;
      }

      return "";
    } catch {
      return "";
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      e.preventDefault();
      handleSubmit();
    }
  };

  const suggestions = [
    'Help me plan a trip',
    'Write a creative story',
    'Explain quantum physics',
    'Create a workout plan'
  ];

  return (

    <div>

      <Navbar /> 

      <div className="chat-container">
        <div className="chat-content">
          <h1 className="chat-title">
            How can I help you today?
          </h1>

          <div className="input-wrapper">
            <div className="input-container">
              <input
                ref={fileInputRef}
                type="file"
                accept=".txt"
                style={{ display: 'none' }}
                onChange={handleFileSelect}
              />

              <button
                className="icon-button"
                aria-label="Add attachment"
                onClick={() => fileInputRef.current?.click()}
              >
                <Plus className="icon" />
              </button>

              <input
                type="text"
                value={url}
                onChange={(e) => setURL(e.target.value)}
                onKeyDown={handleKeyDown}
                placeholder="www.youtube.com/watch?v=example"
                className="message-input"
              />

              <button
                onClick={handleSubmit}
                className="icon-button"
                disabled={loading || (url.length === 0 && courseText.length === 0)}
                aria-label="Send message"
              >
                <Send className="icon" />
              </button>
            </div>
          </div>

          {getYouTubeEmbedUrl() && (
            <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '24px' }}>
              <iframe
                width="560"
                height="315"
                src={getYouTubeEmbedUrl()}
                title="YouTube video preview"
                style={{ borderRadius: '16px', boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)' }}
                allowFullScreen
              ></iframe>
            </div>
          )}

          <div className="suggestions-grid">
            {suggestions.map((suggestion, index) => (
              <button
                key={index}
                onClick={() => setURL(suggestion)}
                className="suggestion-button"
              >
                {suggestion}
              </button>
            ))}
          </div>

          {loading && (
            <div style={{
              position: 'fixed',
              top: 0,
              left: 0,
              right: 0,
              bottom: 0,
              background: 'rgba(0, 0, 0, 0.5)',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              color: 'white',
              fontSize: '20px'
            }}>
              Loading...
            </div>
          )}
        </div>
      </div>

        <ToastContainer />
        {loading && <SpinnerOverlay />}
        <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody}/>
    </div>
  );
}