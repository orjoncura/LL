"use client";
import Button from 'react-bootstrap/Button';
import React, { useState } from 'react';
import Navbar from '@/components/Navbar/Navbar';
import { POST } from '@/scripts/Helpers/SecurityHelper'
import Constants from '@/scripts/Constants'
import {SeminarRequestModel} from '@/generated-client/src';

export default function Home() {

    const [loading, setLoading] = useState(false);
    const [text, setText] = useState<string>('');
    const [response, setResponse] = useState<string | null>(null);
  
    const handleSubmit = async () => {

        try {

            const data: SeminarRequestModel = {
                "text": text,
                "languageFromId": 2,
                "languageToId": 1
            };

            setLoading(true);
            POST('/Seminar/CreateSeminar', JSON.stringify(data))
                .then((results) => {

                    setLoading(false);
                }).catch(e => {
                    setLoading(false);
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
