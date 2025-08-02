"use client";

import React, {useState, useRef, useEffect} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import FeedbackView from '@/components/Feedback/FeedbackView';

import { useRouter } from 'next/navigation';
import { GET } from '@/utils/Security/httpClient'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendar } from '@fortawesome/free-solid-svg-icons';
import { CourseViewModel } from '@/utils/Models/models';
import './page.css'; 

export default function CourseList() {

  const feedbackViewRef = useRef<any>(null); 
  const [loading, setLoading] = useState(true);
  const [courses, setCourses] = useState<CourseViewModel[]>([]);
  const [fbTitle, setfbhTitle] = useState('');
  const [fbBody, setfbhBody] = useState('');
  const [onFeedBackViewClick, setOnFeedBackViewClick] = useState<(() => void) | undefined>(undefined);

  const router = useRouter();

  const showFeedback = (title: string, body:string, onClick?: Function) => {
      if (feedbackViewRef.current) {

          setfbhTitle(title);
          setfbhBody(body);
          setOnFeedBackViewClick(() => onClick); 

          feedbackViewRef.current.open();
      }
  };

  let hasFetchedData = false;
  useEffect(() => {

      if(hasFetchedData == false){
        hasFetchedData = true;

        GET('/Course/GetCourses')
        .then((courseViewModels: CourseViewModel[]) => {

          if(courseViewModels == null || courseViewModels == undefined)
            return;

          setCourses(courseViewModels);
        }).finally(() => {setLoading(false)});
      }
    
  }, []);
  
  const DeleteCourse = async (id: string) => {
      setLoading(true);

      GET('/Course/DeleteCourseById?courseId=' + id)
      .then((hasBeenDeleted: boolean) => {

        if(hasBeenDeleted)
          setCourses(courses.filter(c => c.id != id));
        
        if(hasBeenDeleted == false)
          showFeedback("Error", "Please try again later.");

      }).catch(() => showFeedback("Error", "Something went wrong - Please try again later."))
      .finally(() => setLoading(false));

  };

  const navigateToModule = async (course: CourseViewModel) => {

    localStorage.setItem("SelectedCourse", course.text);
    router.push(`/Course/ModuleNavigator/${course.id}`)
  }

  return (      
    <div>      
      <Navbar /> 
      <br />
      
      <div className="course-list ">
        {courses.map((course, index) => (
          <div key={index} className="position-relative" >
            <button className="close-button" onClick={() => DeleteCourse(course.id)}>
              &times;
            </button>

            <div className="course-card" onClick={() =>  navigateToModule(course)}>
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
    </div>

    {loading && <SpinnerOverlay />}      
    <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody} onClick={onFeedBackViewClick} />
    </div>
  );
};
