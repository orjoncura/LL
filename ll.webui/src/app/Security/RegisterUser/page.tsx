"use client";
import React, {useState, useRef} from 'react';
import { useRouter } from 'next/navigation'
import {Button, Col, Container, Form, Row} from "react-bootstrap";
import { RegisterUser as Register } from '@/utils/Controllers/SecurityController'
import {IsValidEmail} from "@/utils/Security/Validators";
import FeedbackView from '../../../components/Feedback/FeedbackView';
import SpinnerOverlay from '../../../components/Spinner/SpinnerOverlay';
import Link from 'next/link';

export default function RegisterUser() {

    const router = useRouter()
    const [loading, setLoading] = useState(false);
    const feedbackViewRef = useRef<any>(null); 
    const [fbTitle, setfbhTitle] = useState('');
    const [fbBody, setfbhBody] = useState('');
    const [onModalClick, setOnModalClick] = useState<(() => void) | undefined>(undefined);
    const [email, setEmail] = useState('');
    const [confirmEmail, setConfirmEmail] = useState('');
    
    const showFeedback = (title: string, body:string, onClick?: Function) => {
        if (feedbackViewRef.current) {

            setfbhTitle(title);
            setfbhBody(body);
            setOnModalClick(() => onClick); 

            feedbackViewRef.current.open();
        }
    };

    const handleSubmit = (event:any) => {

        console.log("Creating new account...");
        event.preventDefault();

        try {

                if(IsValidEmail(email) == false || IsValidEmail(confirmEmail) == false) {

                    showFeedback("Invalid Email", "Please make sure both emails are valid."); 
                    return;
                }

                if(email != confirmEmail) {

                    showFeedback("Emails don't match", "Please make sure both emails are the same."); 
                    return;
                }

                setLoading(true);
                Register(email, confirmEmail)
                    .then(isSuccessfull => { 
                            if(isSuccessfull) {

                                let fun = () => {
                                    router.push('/', { scroll: false }); 
                                    };

                                showFeedback("Success", "You will receive an email to confirm your identity", fun);
                                
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
                    <h2 className="mainTxt text-center mb-4">Create New User </h2>

                    <Form.Control
                        type="email"
                        placeholder="Email address"
                        className='mainTxt'
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    
                    <br/>
                    <Form.Control
                        type="email"
                        placeholder="Confirm Email"
                        className='mainTxt'
                        value={confirmEmail}
                        onChange={(e) => setConfirmEmail(e.target.value)}
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
            <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody} onClick={onModalClick} />

        </Container>
    );
};
