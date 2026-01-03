"use client";

import React, {useState, useEffect} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';

import { LanguageEnum } from '@/utils/Models/Enums';
import { GetWordsByDifficulty } from '@/utils/Controllers/CourseController'
import { WordViewModel } from '@/utils/Models/models';
import { useParams } from 'next/navigation';

import './page.css'; 

export default function WordsLearned() {
  
    const [loading, setLoading] = useState(false);
    const params = useParams<{ id: string; }>()
    const [words, setWords] = useState<WordViewModel[]>([]);

    let hasFetchedData = false;

    useEffect(() => {

      if(hasFetchedData == false){
        hasFetchedData = true;

        setLoading(true);

        var difficultyId = +params.id;
        GetWordsByDifficulty(LanguageEnum.Spanish, difficultyId)
        .then(words => setWords(words))
        .finally(() => setLoading(false));
      }
    }, []);
    
    return (
      <div style={{ background: 'inherit' }} >
        <Navbar /> 

        <div className="container">
          <div className="word-page">
            <h1 className="title">Vocabulary</h1>

            <div className="frame">
              <div className="grid-header">
                <div>Word</div>
                <div>Translation</div>
              </div>

              <div className="word-grid">
                {words.map((item) => (
                  <React.Fragment key={item.id}>
                    <div className="cell word">{item.name}</div>
                    <div className="cell translation">{item.translation}</div>
                  </React.Fragment>
                ))}
              </div>
            </div>
          </div>
        </div>
        
        {loading && <SpinnerOverlay />}
      </div>
    );
  };