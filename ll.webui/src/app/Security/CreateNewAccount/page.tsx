"use client";
import React, {useState, useRef} from 'react';
import {Button, Col, Container, Form, Row} from "react-bootstrap";
import Constants from "@/scripts/Constants";
import {POST} from "@/scripts/Helpers/SecurityHelper";
import {IsValidEmail} from "@/scripts/Helpers/TextHelper";
import ModalView from '../../../components/Modal/ModalView';

export default function CreateNewAccountPage() {

    const modalRef = useRef<any>(null); 
    const [modalTitle, setModalTitle] = useState('');
    const [modalBody, setModalBody] = useState('');
    const [email, setEmail] = useState('');
    
    const openModal = (title: string, body:string) => {
        if (modalRef.current) {
            setModalTitle(title);
            setModalBody(body);
          modalRef.current.openModal(); // Call openModal from the Example component
        }
      };

    const handleSubmit = (event:any) => {
        event.preventDefault();

        console.log("Creating new account...");

        try {
            if(IsValidEmail(email)) {

                POST('/Security/CreateUserRequest', JSON.stringify(email))
                    .then(data => { })
                    .catch(error => { console.error('Error sending data:', error);});
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

            <ModalView ref={modalRef} modalTitle={modalTitle} modalBody={modalBody} />

        </Container>
    );
};
