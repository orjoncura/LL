"use client";

import React, { useRef, useState, useEffect, ChangeEvent } from 'react';
import { Plus, Send } from 'lucide-react';
import Navbar from '@/components/Navbar/Navbar';
import FeedbackView from '@/components/Feedback/FeedbackView';

import { CourseRequestModel, CourseViewModel, VideoInfo } from '@/utils/Models/models';
import { LanguageEnum } from '@/utils/Models/Enums';
import { SendLocalNotifications } from '@/utils/System/Notification'
import { useToast } from '@/components/Toast/Toast'; 
import { GetCourses, Create } from '@/utils/Controllers/CourseController'
import { useRouter } from 'next/navigation';

import '@/components/Feedback/FeedbackView.css'; 
import './page.css'; 

export default function CreateCourse() {
  const fileInputRef = useRef<HTMLInputElement | null>(null);
  const [courses, setCourses] = useState<CourseViewModel[]>([]);
  const feedbackViewRef = useRef<any>(null); 
  const [loading, setLoading] = useState(false);
  const [fbTitle, setfbhTitle] = useState('');
  const [fbBody, setfbhBody] = useState('');
  const [url, setURL] = useState<string>('');   
  const [courseTitle, setCourseTitle] = useState<string>('');   
  const [courseText, setCourseText] = useState<string>('');   
  const { showToast, ToastContainer } = useToast();

  const router = useRouter();

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

      GetCourses()
      .then((courseViewModels: CourseViewModel[]) => {

        if(courseViewModels == null || courseViewModels == undefined)
          return;

        // API returns newest-first; take the most recent 4.
        setCourses(courseViewModels.slice(0, 4));
      });
    }
  }, []);

  const refreshRecentCourses = () => {
    GetCourses()
      .then((courseViewModels: CourseViewModel[]) => {
        if(courseViewModels == null || courseViewModels == undefined)
          return;

        setCourses(courseViewModels.slice(0, 4));
      });
  };

const handleSubmit = async () => {
  if (loading) return;
  setLoading(true);
  try {
    if (courseText.length === 0 && url.length === 0) 
      throw new Error("Text can not be empty.");
    
    let fetchedTitle = courseTitle;

    if (courseText.length === 0 && url.length > 0) {
      const response = await fetch(`https://www.youtube.com/oembed?url=${encodeURIComponent(url)}&format=json`);
      
      if (!response.ok) {
        throw new Error(`Failed to fetch video info: ${response.status} ${response.statusText}`);
      }
      
      const data: VideoInfo = await response.json();
      setCourseTitle(data.title);
      fetchedTitle = data.title;
    }

    const courseRequestModel: CourseRequestModel = {
      title: fetchedTitle,
      url: url,
      text: courseText,
      languageFromId: LanguageEnum.Spanish,
      languageToId: LanguageEnum.English
    };

    if (!courseRequestModel.title || !courseRequestModel.languageFromId || !courseRequestModel.languageToId) 
      throw new Error("Missing required field in course creation request");

    showToast("Something exciting is coming… a brand new course for " + fetchedTitle + " is on the way!");
    setCourseText("");
    setURL("");
    setCourseTitle("");
    setLoading(false);

    // Course is inserted immediately on the server; refresh suggestions shortly after.
    window.setTimeout(refreshRecentCourses, 1500);

    Create(courseRequestModel)
      .then((isSuccessful) => {
        if (isSuccessful)
          SendLocalNotifications("Your new lesson is ready!!", "Please go to courses to start learning!");
        refreshRecentCourses();
      })
      .catch((error: any) => {
        showFeedback("Error", (error?.message ?? "Course creation failed").substring(0, 100));
      });

  } catch (error: any) {  
    showFeedback("Error", error.message.substring(0, 100));
    setLoading(false);
  }
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

  const navigateToCourse = async (course: CourseViewModel) => {
    if (course.status === 'InProgress' || course.status === 'Failed' || course.hasModules === false)
      return;

    localStorage.setItem("SelectedCourse", (course.text || '').substring(0, 300));
    router.push(`/Course/ModuleNavigator/${course.id}`);
  }

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
            {courses.map((course, index) => (
              <button
                key={index}
                onClick={() => navigateToCourse(course)}
                className="suggestion-button"
              >
                {course.title}
              </button>
            ))}
          </div>

        </div>
      </div>

        <ToastContainer />
        <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody}/>
    </div>
  );
}