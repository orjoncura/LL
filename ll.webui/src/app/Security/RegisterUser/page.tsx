"use client";
import React, {useState, useRef} from 'react';
import { useRouter } from 'next/navigation'
import {Button, Col, Container, Form, Row} from "react-bootstrap";
import {POST} from "@/scripts/Helpers/SecurityHelper";
import {IsValidEmail} from "@/scripts/Helpers/TextHelper";
import ModalView from '../../../components/Modal/ModalView';
import SpinnerOverlay from '../../../components/Spinner/SpinnerOverlay';
import {NewUserModel} from '@/generated-client/src';
import Link from 'next/link';

export default function RegisterUser() {

    const router = useRouter()
    const [loading, setLoading] = useState(false);
    const modalRef = useRef<any>(null); 
    const [modalTitle, setModalTitle] = useState('');
    const [modalBody, setModalBody] = useState('');
    const [onModalClick, setOnModalClick] = useState<(() => void) | undefined>(undefined);

    const [email, setEmail] = useState('');
    const [confirmEmail, setConfirmEmail] = useState('');
    
    const openModal = (title: string, body:string, onClick?: Function) => {
        if (modalRef.current) {

            setModalTitle(title);
            setModalBody(body);
            setOnModalClick(() => onClick); 

          modalRef.current.openModal(); // Call openModal from the Example component
        }
      };

    const handleSubmit = (event:any) => {

        console.log("Creating new account...");
        event.preventDefault();

        try {

                if(IsValidEmail(email) == false || IsValidEmail(confirmEmail) == false) {

                    openModal("Invalid Email", "Please make sure both emails are valid."); 
                    return;
                }

                if(email != confirmEmail) {

                    openModal("Emails don't match", "Please make sure both emails are the same."); 
                    return;
                }

                const data: NewUserModel = {
                    "email": email,
                    "confirmEmail": confirmEmail
                    };

                setLoading(true);
                POST('/Security/RegisterUser', JSON.stringify(data))
                    .then(isSuccessfull => { 
                            if(isSuccessfull) {

                                let fun = () => {
                                    router.push('/', { scroll: false }); 
                                    };

                                openModal("Success", "You will receive an email to confirm your identity", fun);
                                
                            }else{
                                openModal("Error", "Something went wrong the request cannot be completed at this time")
                            }
                        
                            setLoading(false);
                        }).catch(e => { 
                            setLoading(false); 
                            openModal("Error", "The server was unable to complete your request. Please try again later.");
                        });
            
        } catch (error) {
            console.error('Error making API call:', error);
        }
    };
    
    return (
        <Container>
            <Row className="justify-content-md-center mt-5">
                <Col xs={12} md={6}>
                    <h2 className="text-center mb-4">Create New User </h2>

                    <Form.Control
                        type="email"
                        placeholder="Email address"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    
                    <br/>
                    <Form.Control
                        type="email"
                        placeholder="Confirm Email"
                        value={confirmEmail}
                        onChange={(e) => setConfirmEmail(e.target.value)}
                    />

                    <br/>
                    <Button variant="primary" type="submit" className="w-100" onClick={handleSubmit}> Confirm </Button>

                    <hr/>
                    <div className="center">
                        <Link href="/" className='hyperLink'>Sign in</Link>
                    </div>
                </Col>
            </Row>

            {loading && <SpinnerOverlay />}
            <ModalView ref={modalRef} modalTitle={modalTitle} modalBody={modalBody} onClick={onModalClick} />

        </Container>
    );
};
