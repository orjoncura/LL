"use client";
import React, {useState, useEffect} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import Flashcards from '@/components/Courses/Flashcards';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import { GET } from '@/scripts/Helpers/SecurityHelper'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendar } from '@fortawesome/free-solid-svg-icons';
import { CourseViewModel, WordViewModel } from '@/scripts/models';
import './page.css'; 

export default function Profile() {

  const [loading, setLoading] = useState(true);
  const [courses, setCourses] = useState<CourseViewModel[]>([]);
  const [showCourse, setShowCourse] = useState(false);
  const [wordViewModels, setWordViewModels] = useState<WordViewModel[]>([]); 
  const [courseText, setCourseText] = useState<string>('');   

  useEffect(() => {
      GET('/Course/GetCourses')
      .then((courseViewModels: CourseViewModel[]) => {

        if(courseViewModels == null || courseViewModels == undefined)
          return;

        setCourses(courseViewModels);
      }).finally(() => {setLoading(false)});
    
  }, []);
  
  const handleSubmit = async (id: number, content:string) => {
      setLoading(true);

      GET('/Course/GetCourseWords?courseId=' + id)
      .then((wordViewModels: WordViewModel[]) => {

        if(wordViewModels == null || wordViewModels == undefined)
          return;

        setWordViewModels(wordViewModels);
      }).finally(() => {
          setCourseText(content);
          setLoading(false);
          setShowCourse(true);
        });

  };

  return (      
    <div>      
      <Navbar /> 
      <br />
      {showCourse == false && ( 
      <div className="course-list responsive-padding">
        {courses.map((course, index) => (
          <div key={index} className="course-card" onClick={() => handleSubmit(course.id, course.text)}>
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
        ))}
    </div>)}

    {showCourse && (<Flashcards text={courseText} words={wordViewModels} onDone={() => setShowCourse(false)} />)}

    {loading && <SpinnerOverlay />}
    </div>
  );
};
