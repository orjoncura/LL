"use client";
import React, {useState, useEffect} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import { GET } from '@/scripts/Helpers/SecurityHelper'
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendar } from '@fortawesome/free-solid-svg-icons';
import './page.css'; 
import { CourseViewModel } from '@/scripts/models';

export default function Profile() {

  const [loading, setLoading] = useState(false);
  const [courses, setCourses] = useState<CourseViewModel[]>([]);

  useEffect(() => {
      setLoading(true);

      GET('/Course/GetCourses')
      .then((courseViewModels: CourseViewModel[]) => {

        if(courseViewModels == null || courseViewModels == undefined)
          return;

        setCourses(courseViewModels);
      }).finally(() => {setLoading(false)});
    
  }, []);
  
  return (      
    <div>      
      <Navbar /> 
      <br />
      <div className="course-list responsive-padding">
        {courses.map((course, index) => (
          <div className="course-card">
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
    </div>

    {loading && <SpinnerOverlay />}
    </div>
  );
};
