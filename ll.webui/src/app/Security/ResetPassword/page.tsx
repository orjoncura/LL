"use client";
import React, {useState, useRef} from 'react';
import { useRouter } from 'next/navigation'
import {Button, Col, Container, Form, Row} from "react-bootstrap";
import { ResetPassword as Reset } from '@/utils/Controllers/SecurityController'
import {getEnv} from '@/utils/Models/EnvironmentVariables';
import {IsValidEmail} from "@/utils/Security/Validators";
import FeedbackView from '../../../components/Feedback/FeedbackView';
import SpinnerOverlay from '../../../components/Spinner/SpinnerOverlay';

const applicationName = getEnv().Application_Name || '';

export default function ResetPassword() {

    const router = useRouter()
    const [loading, setLoading] = useState(false);
    const feedbackViewRef = useRef<any>(null); 
    const [fbTitle, setfbhTitle] = useState('');
    const [fbBody, setfbhBody] = useState('');
    const [onFeedBackViewClick, setOnFeedBackViewClick] = useState<(() => void) | undefined>(undefined);

    const [email, setEmail] = useState('');
    
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
            if(IsValidEmail(email)) {

                setLoading(true);
                Reset(email)
                    .then(isSuccessfull => { 

                        if(isSuccessfull) {

                            let fun = () => {
                                router.push('/', { scroll: false }); 
                            };

                            showFeedback("Success", "You will receive an email to confirm your request", fun);
                            
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
        <Container className='mainTxt'>
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
                    <Button variant="success" type="submit" className="mainTxt w-100" onClick={handleSubmit}> Submit </Button>
                </Col>
            </Row>

            {loading && <SpinnerOverlay />}
            <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody} onClick={onFeedBackViewClick} />

        </Container>
    );
};
