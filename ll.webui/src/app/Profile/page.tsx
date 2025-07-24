"use client";

import React, {useState, useRef, useEffect} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import Flashcards from '@/components/Courses/Flashcards';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import FeedbackView from '@/components/Feedback/FeedbackView';

import { GET } from '@/utils/Security/httpClient'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendar } from '@fortawesome/free-solid-svg-icons';
import { CourseViewModel, WordViewModel } from '@/utils/Models/models';

import './page.css'; 

export default function Profile() {

  const [loading, setLoading] = useState(true);
  const [courses, setCourses] = useState<CourseViewModel[]>([]);
  const [showCourse, setShowCourse] = useState(false);
  const [wordViewModels, setWordViewModels] = useState<WordViewModel[]>([]); 
  const [selectedCourse, setSelectedCourse] = useState<CourseViewModel>(courses[0]);   
  const feedbackViewRef = useRef<any>(null); 
  const [fbTitle, setfbhTitle] = useState('');
  const [fbBody, setfbhBody] = useState('');
  const [onFeedBackViewClick, setOnFeedBackViewClick] = useState<(() => void) | undefined>(undefined);

  const showFeedback = (title: string, body:string, onClick?: Function) => {
      if (feedbackViewRef.current) {

          setfbhTitle(title);
          setfbhBody(body);
          setOnFeedBackViewClick(() => onClick); 

          feedbackViewRef.current.open();
      }
  };

  useEffect(() => {
      GET('/Course/GetCourses')
      .then((courseViewModels: CourseViewModel[]) => {

        if(courseViewModels == null || courseViewModels == undefined)
          return;

        setCourses(courseViewModels);
      }).finally(() => {setLoading(false)});
    
  }, []);
  
  const handleSubmit = async (course: CourseViewModel) => {
      setLoading(true);

      GET('/Course/GetCourseWords?courseId=' + course.id)
      .then((wordViewModels: WordViewModel[]) => {

        if(wordViewModels == null || wordViewModels == undefined)
          return;

        setWordViewModels(wordViewModels);

        window.history.pushState({ modalOpen: true }, "", "Profile/Flashcards");
      }).finally(() => {
          setSelectedCourse(course);
          setLoading(false);
          setShowCourse(true);
        });

  };

    const DeleteCourse = async (id: number) => {
      setLoading(true);

      GET('/Course/DeleteCourseById?courseId=' + id)
      .then((hasBeenDeleted: boolean) => {

        if(hasBeenDeleted)
          setCourses(courses.filter(c => c.id != id));
        
        if(hasBeenDeleted == false)
          showFeedback("Error", "Please try again later.");

        setWordViewModels(wordViewModels);
      }).catch(() => showFeedback("Error", "Something went wrong - Please try again later."))
      .finally(() => setLoading(false));

  };

  return (      
    <div>      
      <Navbar /> 
      <br />
      {showCourse == false && ( 
      <div className="course-list ">
        {courses.map((course, index) => (
          <div key={index} className="position-relative" >
            <button className="close-button" onClick={() => DeleteCourse(course.id)}>
              &times;
            </button>

            <div className="course-card" onClick={() => handleSubmit(course)}>
              <div className="course-header">
                <div className="course-title">
                  {course.text.substring(0, 25)}  
                </div>
                <div className="course-date"><FontAwesomeIcon icon={faCalendar} className="icon" /> <span>{course.createdDate.asString}</span></div>
              </div>
            <hr className="course-divider" />
            <div className="course-detail">
              {<span>{course.text.substring(0, 300)}</span> }
            </div>
          </div>
          </div>

        ))}
    </div>)}

    {showCourse && (<Flashcards text={selectedCourse.text} words={wordViewModels} courseId={selectedCourse.id} onDone={() => setShowCourse(false)} />)}

    {loading && <SpinnerOverlay />}      
    <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody} onClick={onFeedBackViewClick} />
    </div>
  );
};
