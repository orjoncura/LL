"use client";
import React, {useState, useRef} from 'react';
import { useRouter } from 'next/navigation'
import {Button, Col, Container, Form, Row} from "react-bootstrap";
import Constants from "@/scripts/Constants";
import {POST} from "@/scripts/Helpers/SecurityHelper";
import {IsValidEmail} from "@/scripts/Helpers/TextHelper";
import ModalView from '../../../components/Modal/ModalView';
import SpinnerOverlay from '../../../components/Spinner/SpinnerOverlay';

export default function ResetPassword() {

    const router = useRouter()
    const [loading, setLoading] = useState(false);
    const modalRef = useRef<any>(null); 
    const [modalTitle, setModalTitle] = useState('');
    const [modalBody, setModalBody] = useState('');
    const [onModalClick, setOnModalClick] = useState<(() => void) | undefined>(undefined);

    const [email, setEmail] = useState('');
    
    const openModal = (title: string, body:string, onClick?: Function) => {
        if (modalRef.current) {

            setModalTitle(title);
            setModalBody(body);
            setOnModalClick(() => onClick); 

          modalRef.current.openModal(); // Call openModal from the Example component
        }
      };

    const handleSubmit = (event:any) => {
        event.preventDefault();

        console.log("Creating new account...");

        try {
            if(IsValidEmail(email)) {

                setLoading(true);
                POST('/Security/ResetPassword', JSON.stringify(email))
                    .then(isSuccessfull => { 

                        if(isSuccessfull) {

                            let fun = () => {
                                router.push('/', { scroll: false }); 
                                };

                            openModal("Success", "You will receive an email to confirm your request", fun);
                            
                        }else{
                            openModal("Error", "Something went wrong the request cannot be completed at this time");
                        }
                        
                        setLoading(false);
                    }).catch(error => { 
                        setLoading(false); 
                        openModal("Error", error.message);
                    });
            }else {
                openModal("Invalid Email", "Please pass a valid email"); 
            }
        } catch (error) {
            console.error('Error making API call:', error);
        }
    };
    
    return (
        <Container>
            <Row className="justify-content-md-center mt-5">
                <Col xs={12} md={6}>
                    <h2 className="text-center mb-4">{Constants().ApplicationName}</h2>

                    <Form.Control
                        type="email"
                        placeholder="Email address"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    
                    <br/>
                    <Button variant="success" type="submit" className="w-100" onClick={handleSubmit}> Submit </Button>
                </Col>
            </Row>

            {loading && <SpinnerOverlay />}
            <ModalView ref={modalRef} modalTitle={modalTitle} modalBody={modalBody} onClick={onModalClick} />

        </Container>
    );
};
