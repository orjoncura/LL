"use client";
import Button from 'react-bootstrap/Button';
import React, { useState } from 'react';
import Navbar from '@/components/Navbar/Navbar/Navbar';
import Constants from '@/scripts/Constants'

export default function Home() {
  const [text, setText] = useState<string>('');
  const [response, setResponse] = useState<string | null>(null);

    function createListFromText(value: string) {
        // Remove special symbols using a regular expression
        const cleanedArticle = value.replace(/[^\w\s.]/g, '');

        const sentences = cleanedArticle.split(' ').map(sentence => sentence.trim()).filter(sentence => sentence.length > 0);

        return sentences;
    }

  const handleSubmit = async () => {

    try {
        const data = {
            "words": createListFromText(text),
            "languageIdFrom": 1,
            "langaugeIdTo": 2
        };

        fetch(Constants().API + '/Seminar/CreateSeminar', { 
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
