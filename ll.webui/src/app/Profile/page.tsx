"use client";
import React from 'react';
import Navbar from '@/components/Navbar/Navbar';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendar } from '@fortawesome/free-solid-svg-icons';
import './page.css'; 

export default function Profile() {

  const courses = [
    {
      title: "Lorem Ipsum is simply dummy text of the printing and typesetting industry. L",
      time: "24/03/2025",
      sample: "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of de Finibus Bonorum et Malorum (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, Lorem ipsum dolor sit amet.., comes from a line in section 1.10.32."
    },
    {
      title: "Para cosas buenas",
      time: "24/03/2025",
      sample: "Vine a cumplir una misión (una misión)"
    },
    {
      title: "Title",
      time: "24/03/2025",
      sample: "sample"
    }
  ];
  

  return (      
    <div>      
      <Navbar /> 
      <br />
      <div className="course-list responsive-padding">
        {courses.map((course, index) => (
          <div className="course-card">
            <div className="course-header">
              <div className="course-title">
                {course.title.substring(0, 25)} 
              </div>
              <div className="course-date"><FontAwesomeIcon icon={faCalendar} className="icon" /> <span>{course.time}</span></div>
            </div>
          <hr className="course-divider" />
          <div className="course-detail">
             <span>{course.sample.substring(0, 300)}</span>
          </div>
        </div>
        ))}
    </div>
    </div>
  );
};
