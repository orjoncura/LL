"use client";

import React, {useState, useRef, useEffect} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import FeedbackView from '@/components/Feedback/FeedbackView';

import { useRouter } from 'next/navigation';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendar } from '@fortawesome/free-solid-svg-icons';
import { CourseViewModel } from '@/utils/Models/models';
import { GetCourses, DeleteCourseById } from '@/utils/Controllers/CourseController'


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

        GetCourses()
        .then((courseViewModels: CourseViewModel[]) => {

          if(courseViewModels == null || courseViewModels == undefined)
            return;

          setCourses(courseViewModels);
        }).finally(() => {setLoading(false)});
      }
    
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

    localStorage.setItem("SelectedCourse", course.text.substring(0, 300));
    router.push(`/Course/ModuleNavigator/${course.id}`);
  }

  return (
    <div>
      <Navbar/> 
      <div style={{
        minHeight: '100vh',
        background: 'linear-gradient(135deg, #f3e8ff 0%, #fae8ff 50%, #fdf4ff 100%)',
        padding: '48px 32px'
      }}>
        <div style={{
          maxWidth: '1024px',
          margin: '0 auto'
        }}>
          <h1 style={{ marginBottom: '32px' }}>Available Courses</h1>
          <div style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
            {courses.map((course) => (
              <div
                key={course.id}
                style={{
                  background: 'white',
                  borderRadius: '12px',
                  padding: '24px',
                  boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)',
                  border: '1px solid rgba(0, 0, 0, 0.05)',
                  transition: 'box-shadow 0.2s',
                  position: 'relative'
                }}
              >
                <button onClick={() => DeleteCourse(course.id)}
                style={{
                  position: 'absolute',
                  top: '16px',
                  right: '16px',
                  background: 'transparent',
                  color: '#9ca3af',
                  border: '1px solid #e5e7eb',
                  borderRadius: '6px',
                  padding: '6px 12px',
                  cursor: 'pointer',
                  fontSize: '12px',
                  fontWeight: '400'
                }}>
                  Archive
                </button>
                <h3 style={{ marginBottom: '8px', paddingRight: '80px' }}>{course.title}</h3>
                <p style={{ color: '#6b7280', marginBottom: '16px' }}>{course.text.substring(0, 300)}</p>
                <button onClick={() =>  navigateToModule(course)}
                  style={{
                    background: 'linear-gradient(90deg, #7c3aed 0%, #d946ef 100%)',
                    color: 'white',
                    border: 'none',
                    borderRadius: '8px',
                    padding: '10px 20px',
                    cursor: 'pointer',
                    fontSize: '14px',
                    fontWeight: '500'
                }}>Start
                </button>
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