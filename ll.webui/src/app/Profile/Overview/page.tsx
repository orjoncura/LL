"use client";

import React, {useState, useRef, useEffect} from 'react';
import Navbar from '@/components/Navbar/Navbar';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import FeedbackView from '@/components/Feedback/FeedbackView';
import Link from 'next/link';

import { GET, POST } from '@/utils/Security/httpClient'
import { ProfileViewModel, GetWordsByDifficultyModel } from '@/utils/Models/models';
import { ImportanceRatingEnum, LanguageEnum } from '@/utils/Models/Enums';
import { IsValidEmail } from "@/utils/Security/Validators";

import './page.css'; 

export default function Overview() {

    const [profile, setProfile] = useState<ProfileViewModel | null>(null);  
    const [loading, setLoading] = useState(false);
    const feedbackViewRef = useRef<any>(null); 
    const [fbTitle, setfbhTitle] = useState('');
    const [fbBody, setfbhBody] = useState('');
    const [onFeedBackViewClick, setOnFeedBackViewClick] = useState<(() => void) | undefined>(undefined);
    const [easyWordsLearned, setEasyWordsLearned] = useState<number>(0);
    const [mediumWordsLearned, setMediumWordsLearned] = useState<number>(0);
    const [hardWordsLearned, setHardWordsLearned] = useState<number>(0);

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

        setLoading(true);

        var url: string = '/Course/GetWordCountByDifficulty';
        POST(url, JSON.stringify({"languageId": LanguageEnum.Spanish, "difficultyId": ImportanceRatingEnum.Low}))
        .then(wordCount => setEasyWordsLearned(wordCount));

        POST(url, JSON.stringify({"languageId": LanguageEnum.Spanish, "difficultyId": ImportanceRatingEnum.Medium}))
        .then(wordCount => setMediumWordsLearned(wordCount));

        POST(url, JSON.stringify({"languageId": LanguageEnum.Spanish, "difficultyId": ImportanceRatingEnum.High}))
        .then(wordCount => setHardWordsLearned(wordCount));

        GET('/Security/GetProfileDetails')
        .then((profileViewModel: ProfileViewModel) => {
  
          if(profileViewModel == null || profileViewModel == undefined)
            return;
  
          setProfile(profileViewModel);
        }).finally(() => {setLoading(false)});

      }
    }, []);

    const handleSubmit = (event:any) => {
        event.preventDefault();

        if(profile == null)  return;

        try {
            if(IsValidEmail(profile?.email)) {

                setLoading(true);
                POST('/Security/ResetPassword', JSON.stringify(profile?.email))
                    .then(isSuccessfull => { 

                        if(isSuccessfull) {

                            showFeedback("Success", "You will receive an email to confirm your request");
                            
                        }else{
                            showFeedback("Error", "Something went wrong the request cannot be completed at this time");
                        }
                        
                        setLoading(false);
                    }).catch(e => { 
                        setLoading(false); 
                        showFeedback("Error", "The server was unable to complete your request. Please try again later.");
                    });
            }else {
                showFeedback("Invalid Email", "Please pass a valid email"); 
            }
        } catch (error) {
            console.error('Error making API call:', error);
        }
    };
            
    return (
      <div>

        <Navbar /> 

        <div className="container-flex">
          <div className="container" style={{ width: "350px" }} >
            <div className='glow-frame' style={{ marginBottom: "25%" }}>
                <div style={{ textAlign: 'center', marginTop: '1rem' }}>
                  <b className='mainTxt'>{profile?.email}</b>
                  <br></br>
                  <br></br>
                  <b className='mainTxt'>Joined: {profile?.dateCreated.asString}</b>
                  <br></br>
                  <br></br>
                </div> 
                <button className="mainBtn w-100" onClick={handleSubmit}>Reset Password</button>
              </div>

              <div className="words-learned" > 
                <Link className='mainTxt' href="/Profile/WordsLearned/3"> Easy words learned {easyWordsLearned}</Link>
              </div>

              <div className="words-learned" > 
                <Link className='mainTxt' href="/Profile/WordsLearned/2"> Medium words learned {mediumWordsLearned}</Link>
              </div>

              <div className="words-learned" > 
                <Link className='mainTxt' href="/Profile/WordsLearned/1"> Hard words learned {hardWordsLearned}</Link>
              </div>
            </div>
        </div>

        <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody} onClick={onFeedBackViewClick} />
        {loading && <SpinnerOverlay />}
        
      </div>
    );
  };
