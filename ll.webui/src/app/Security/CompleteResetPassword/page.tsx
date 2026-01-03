"use client";
import React, { useState, useRef } from 'react';
import { useRouter } from 'next/navigation'
import { Form, Button, Container, Row, Col } from 'react-bootstrap';
import { CompletePasswordReset } from '@/utils/Controllers/SecurityController'
import {IsValidPassword, IsValidEmail} from "@/utils/Security/Validators";
import FeedbackView from '@/components/Feedback/FeedbackView';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import {ConfirmationModel} from '@/utils/Models/models';
import Link from 'next/link';

export default function CompleteResetPassword() {

    const router = useRouter()
    const [loading, setLoading] = useState(false);
    const feedbackViewRef = useRef<any>(null); 
    const [fbTitle, setfbhTitle] = useState('');
    const [fbBody, setfbhBody] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirPassword] = useState('');
    const [onFeedBackViewClick, setOnFeedBackViewClick] = useState<(() => void) | undefined>(undefined);

    const showFeedback = (title: string, body:string, onClick?: Function) => {
        if (feedbackViewRef.current) {

            setfbhTitle(title);
            setfbhBody(body);
            setOnFeedBackViewClick(() => onClick); 

            feedbackViewRef.current.open();
        }
    };

    const handleSubmit = (event:any) => {
        event.preventDefault();
    
        try {
              if(IsValidPassword(password) == false || IsValidPassword(confirmPassword) == false) {

                showFeedback("Invalid Password", "Please make sure both passwords are valid."); 
                return;
              }

              if(password != confirmPassword) {

                  showFeedback("Passwords don't match", "Please make sure both passwords are the same."); 
                  return;
              }
            
              const searchParams = new URLSearchParams(window.location.search);
              let token:string = searchParams.get('token') || '';
              let email:string = searchParams.get('email') || '';

              if(token.length < 1) {

                showFeedback("Invalid Token", "The token provided is invalid or has expired. Please request a new one.");
                return;
              }
              
              if(IsValidEmail(email) == false) {

                showFeedback("Invalid Link", "This link is invalid or has expired. Please request a new one.");
                return;
              }

              const data: ConfirmationModel = {
                "password": password,
                "confirmPassword": confirmPassword,
                "token": token,
                "email": email
              };
              
              setLoading(true);
              CompletePasswordReset(data)
                .then(isSuccessfull => { 
                  if(isSuccessfull) {

                      let fun = () => {
                          router.push('/', { scroll: false }); 
                        };

                      showFeedback("Success", "Your password has been reset successfully", fun);
                      
                  }else{
                      showFeedback("Error", "Something went wrong the request cannot be completed at this time")
                  }
                
                  setLoading(false);
                }).catch(e => { 
                  setLoading(false); 
                  showFeedback("Error", "The server was unable to complete your request. Please try again later.");
              });

        } catch (error) {
          console.error('Error making API call:', error);
        }
      };

    return (
        <Container>
          <Row className="justify-content-md-center mt-5">
              <Col xs={12} md={6}>
                  <h2 className="mainTxt text-center mb-4">Reset Password</h2>
    
                  <Form.Control
                      type="password"
                      placeholder="Password"
                      className='mainTxt'
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
                  />
                  
                  <br/>
                  <Form.Control
                      type="password"
                      placeholder="Confirm Password"
                      className='mainTxt'
                      value={confirmPassword}
                      onChange={(e) => setConfirPassword(e.target.value)}
                  />
        
                  <br/>
                  <Button variant="primary" type="submit" className="mainBtn w-100" onClick={handleSubmit}> Confirm </Button>

                  <hr/>
                  <div className="center">
                      <Link href="/" className='mainTxt hyperLink'>Sign in</Link>
                  </div>
              </Col>
          </Row>

          {loading && <SpinnerOverlay />}
          <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody} onClick={onFeedBackViewClick} />
      </Container>
    );
};
