"use client";
import Button from 'react-bootstrap/Button';
import React, { useState } from 'react';
import Navbar from '@/components/Navbar/Navbar/Navbar';

export default function Home() {
  const [text, setText] = useState<string>('');
  const [response, setResponse] = useState<string | null>(null);

  const handleSubmit = async () => {

    try {
        const data = {
            "words": [
                "string"
            ],
            "languageIdFrom": 0,
            "langaugeIdTo": 0
        };

        fetch('http://localhost:5197/api/Seminar/CreateSeminar', { 
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
        .then(response => response.json())
        .then(data => {
            console.log('Data sent successfully:', data);
        })
        .catch(error => {
            console.error('Error sending data:', error);
        });

        const response = await fetch('http://localhost:5197/api/Seminar/CreateSeminar');

        if (!response.ok) throw new Error('Network response was not ok');

        const result = await response.json();

        setResponse(result);

    } catch (error) {
      console.error('Error making API call:', error);
      setResponse('An error occurred');
    }
  };

    return (
      <div style={{ background: 'inherit' }} >
      <Navbar /> 
      <br />
      <div className="container">
        <textarea
          value={text}
          onChange={e => setText(e.target.value)}
          placeholder="Enter your text here..."
          rows={10}
          cols={50}
          style={{ marginBottom: '10px', width: '100%' }}
        />
        <br />
        <Button variant="success" onClick={handleSubmit}>Submit</Button>
        {response && <p>Response: {response}</p>}
      </div>
    </div>
    );
  };
