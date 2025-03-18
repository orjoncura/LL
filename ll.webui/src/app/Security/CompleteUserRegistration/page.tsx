"use client";
import React, { useState, useRef } from 'react';
import { useRouter } from 'next/navigation'
import { Form, Button, Container, Row, Col } from 'react-bootstrap';
import {POST} from "@/scripts/Helpers/SecurityHelper";
import {IsValidPassword, IsValidEmail} from "@/scripts/Helpers/TextHelper";
import ModalView from '@/components/Modal/ModalView';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import {ConfirmationModel} from '@/scripts/models';
import Link from 'next/link';

export default function CompleteUserRegistration() {

    const router = useRouter()
    const [loading, setLoading] = useState(false);
    const modalRef = useRef<any>(null); 
    const [modalTitle, setModalTitle] = useState('');
    const [modalBody, setModalBody] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirPassword] = useState('');
    const [onModalClick, setOnModalClick] = useState<(() => void) | undefined>(undefined);

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
    
        try {
              if(IsValidPassword(password) == false || IsValidPassword(confirmPassword) == false) {

                openModal("Invalid Password", "Please make sure that the password contains a capital letter and a symbol."); 
                return;
              }

              if(password != confirmPassword) {

                  openModal("Passwords don't match", "Please make sure both passwords are the same."); 
                  return;
              }
            
              const searchParams = new URLSearchParams(window.location.search.toLowerCase());
              let token:string = searchParams.get('token') || '';
              let email:string = searchParams.get('email') || '';

              if(token.length < 1) {

                openModal("Invalid Token", "The token provided is invalid or has expired. Please request a new one.");
                return;
              }

              if(IsValidEmail(email) == false) {

                openModal("Invalid Link", "This link is invalid or has expired. Please request a new one.");
                return;
              }

              const data: ConfirmationModel = {
                "password": password,
                "confirmPassword": confirmPassword,
                "token": token,
                "email": email
              };
              
              setLoading(true);
              POST('/Security/CompleteUserRegistration', JSON.stringify(data))
                .then(isSuccessfull => { 

                  if(isSuccessfull) {

                      let fun = () => {
                          router.push('/', { scroll: false }); 
                        };

                      openModal("Success", "Your new account has been created successfully", fun);
                      
                  }else{                      
                      openModal("Error", "Something went wrong the request cannot be completed at this time. "
                        + "If you already created an account please reset your password");
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
                    <h2 className="mainTxt text-center mb-4">User Registration</h2>

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
                    <Button variant="primary" type="submit" className="mainTxt w-100" onClick={handleSubmit}> Confirm </Button>

                    <hr/>
                    <div className="center">
                        <Link href="/" className='mainTxt hyperLink'>Sign in</Link>
                    </div>
                </Col>
            </Row>

            {loading && <SpinnerOverlay />}
            <ModalView ref={modalRef} modalTitle={modalTitle} modalBody={modalBody} onClick={onModalClick}/>
        </Container>
    );
};
