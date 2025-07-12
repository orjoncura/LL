"use client";
import React, {useState, useRef, useEffect} from 'react';
import { Form, Button, Container, Row, Col } from 'react-bootstrap';
import { useRouter } from 'next/navigation'
import { POST } from '@/utils/Security/httpClient'
import { StoreToken } from '@/utils/Security/AuthManager'
import {getEnv} from '@/utils/Models/EnvironmentVariables';
import { IsValidEmail, IsValidPassword } from "@/utils/Security/Validators";
import {LoginModel, TokenViewModel} from '@/utils/Models/models';
import Link from 'next/link';
import FeedbackView from '../components/Feedback/FeedbackView';
import SpinnerOverlay from '../components/Spinner/SpinnerOverlay';
import './globals.css'; 

const applicationName = getEnv().Application_Name || '';

export default function Login() {

    const router = useRouter()
    const [loading, setLoading] = useState(false);
    const feedbackViewRef = useRef<any>(null); 
    const [fbTitle, setfbhTitle] = useState('');
    const [fbBody, setfbhBody] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [onFeedBackViewClick, setOnFeedBackViewClick] = useState<(() => void) | undefined>(undefined);

    useEffect(() => { console.log("Version:" + getEnv().Version)}, []);

    const showFeedback = (title: string, body:string, onClick?: Function) => {
        if (feedbackViewRef.current) {

            setfbhTitle(title);
            setfbhBody(body);
            setOnFeedBackViewClick(() => onClick); 

            feedbackViewRef.current.open();
        }
    };

    const handleSubmit = (event: any) => {

        event.preventDefault();
  
        try {
            if (IsValidEmail(email) == false) {

                showFeedback("Error", "Invalid email address format.");
                return;
            }

            if (IsValidPassword(password) == false) {

                showFeedback("Error", "Invalid password format.");
                return;
            }
  
            const data: LoginModel = {
                "email": email,
                "password": password
            };
  
            setLoading(true);
            POST('/Security/Authenticate', JSON.stringify(data))
                .then((tokenModel: TokenViewModel) => {

                    if (tokenModel.token != null && tokenModel.token.length > 1) {

                        StoreToken(tokenModel.token);
                        router.push('/Course/Create', { scroll: false });
  
                    } else {
                        showFeedback("Error", "It looks like the username or password you entered doesn't match our records." 
                        + " Please double - check and try again.");
                    }
  
                    setLoading(false);
                }).catch(e => {
                    setLoading(false);
                    showFeedback("Error", "The server was unable to complete your request. Please try again later.");
                });
            
  
        } catch (error) {
            console.error('Error making API call:', error);
        };
    }

  return (
    <Container onKeyDown={(e) => {
        if (e.key === "Enter" && !e.shiftKey && "form" in e.target) {
          handleSubmit(e);
        }
      }}>

      <Row className="justify-content-md-center mt-5">
          <Col xs={12} md={6}>
              <h2 className="mainTxt text-center mb-4">{applicationName}</h2>

              <Form.Control
                  type="email"
                  placeholder="Email address"
                  className='mainTxt'
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
              />
              
              <br/>
              <Form.Control
                  type="password"
                  placeholder="Password"
                  className='mainTxt'
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
              />
              <br/>
              <Button type="submit" className="mainBtn w-100" onClick={handleSubmit}> Login </Button>

              <hr/>
              <div className="center">
                  <Link href="/Security/ResetPassword" className='mainTxt hyperLink'>Forgotten password?</Link>
              </div>

              <br/>
              <Link href="/Security/RegisterUser" className="mainTxt w-100 button-link btn btn-success">
                  Create new account
              </Link>
          </Col>
      </Row>
      {loading && <SpinnerOverlay />}
      <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody} onClick={onFeedBackViewClick} />
    </Container>
  );
}