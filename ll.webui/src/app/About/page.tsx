"use client";
import React from 'react';
import Link from 'next/link';
import Navbar from '@/components/Navbar/Navbar';

export default function About() {
  return (      
    <div>      
      <Navbar /> 
      <br />
      <h1>About Us</h1>
      <p>
        Welcome to our website! We are dedicated to providing the best content for our users.
        Our team works tirelessly to bring you the latest and greatest information.
      </p>
      <p>
        This website is a place where we share our thoughts, experiences, and knowledge on various topics.
        We hope you find the content useful and engaging.
      </p>
      <Link href="/">About</Link>
    </div>
  );
};
