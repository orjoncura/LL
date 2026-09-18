"use client";

import React, {useState, useRef, useEffect} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import FeedbackView from '@/components/Feedback/FeedbackView';

import { useRouter } from 'next/navigation';
import { CourseViewModel } from '@/utils/Models/models';
import { GetCourses, DeleteCourseById } from '@/utils/Controllers/CourseController'

import './page.css';

function getCourseStatus(course: CourseViewModel): 'Ready' | 'InProgress' | 'Failed' {
  if (course.status === 'Ready' || course.status === 'InProgress' || course.status === 'Failed')
    return course.status;

  if (course.hasModules)
    return 'Ready';

  if (course.createdAt) {
    const createdAtMs = Date.parse(course.createdAt);
    if (!Number.isNaN(createdAtMs) && Date.now() - createdAtMs >= 10 * 60 * 60 * 1000)
      return 'Failed';
  }

  if (course.hasModules === false)
    return 'InProgress';

  return 'Ready';
}

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

  const loadCourses = (showSpinner = false) => {
    if (showSpinner) setLoading(true);

    return GetCourses()
      .then((courseViewModels: CourseViewModel[]) => {
        if (courseViewModels == null || courseViewModels == undefined)
          return;

        setCourses(courseViewModels);
      })
      .finally(() => {
        if (showSpinner) setLoading(false);
      });
  };

  let hasFetchedData = false;
  useEffect(() => {

      if(hasFetchedData == false){
        hasFetchedData = true;
        loadCourses(true);
      }
    
  }, []);

  const hasInProgressCourses = courses.some(course => getCourseStatus(course) === 'InProgress');

  useEffect(() => {
    if (!hasInProgressCourses) return;

    const intervalId = window.setInterval(() => {
      loadCourses(false);
    }, 4000);

    return () => window.clearInterval(intervalId);
  }, [hasInProgressCourses]);

  useEffect(() => {
    const refreshOnFocus = () => loadCourses(false);
    const onVisibilityChange = () => {
      if (document.visibilityState === 'visible')
        refreshOnFocus();
    };

    window.addEventListener('focus', refreshOnFocus);
    document.addEventListener('visibilitychange', onVisibilityChange);

    return () => {
      window.removeEventListener('focus', refreshOnFocus);
      document.removeEventListener('visibilitychange', onVisibilityChange);
    };
  }, []);
  
  const DeleteCourse = async (id: string) => {
      setLoading(true);

      DeleteCourseById(id)
      .then((hasBeenDeleted: boolean) => {

        if(hasBeenDeleted)
          setCourses(courses.filter(c => c.id != id));
        
        if(hasBeenDeleted == false)
          showFeedback("Error", "Please try again later.");

      }).catch(() => showFeedback("Error", "Something went wrong - Please try again later."))
      .finally(() => setLoading(false));

  };

  const navigateToModule = async (course: CourseViewModel) => {
    if (getCourseStatus(course) !== 'Ready') return;

    localStorage.setItem("SelectedCourse", (course.text || '').substring(0, 300));
    router.push(`/Course/ModuleNavigator/${course.id}`);
  }

  const renderCourseAction = (course: CourseViewModel) => {
    const status = getCourseStatus(course);

    if (status === 'InProgress')
      return <span className="status-in-progress">In progress</span>;

    if (status === 'Failed')
      return <span className="status-failed">Failed</span>;

    return (
      <button className="course-start-btn" onClick={() => navigateToModule(course)}>
        Start
      </button>
    );
  };

  return (
    <div>
      <Navbar/> 
      <div className="course-list-page">
        <div className="course-list-inner">
          <h1 className="course-list-title mainTxt">Available Courses</h1>
          <div className="course-list-items">
            {courses.map((course) => (
              <div key={course.id} className="course-card-item">
                <button className="course-archive-btn" onClick={() => DeleteCourse(course.id)}>
                  Archive
                </button>
                <h3 className="course-card-title">{course.title}</h3>
                <p className="course-card-text">{(course.text || '').substring(0, 300)}</p>
                {renderCourseAction(course)}
              </div>
            ))}
          </div>
        </div>
      </div>
      {loading && <SpinnerOverlay />}      
      <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody} onClick={onFeedBackViewClick} />
    </div>
  );
}
