"use client";
import React, { useState, useRef } from 'react';
import { useRouter } from 'next/navigation'
import { Form, Button, Container, Row, Col } from 'react-bootstrap';
import {POST} from "@/scripts/Helpers/SecurityHelper";
import {IsValidPassword} from "@/scripts/Helpers/TextHelper";
import ModalView from '../../../components/Modal/ModalView';
import {NewPasswordModel} from '@/generated-client/src';
import Link from 'next/link';

export default function CompleteResetPassword() {

    const router = useRouter()
    const modalRef = useRef<any>(null); 
    const [modalTitle, setModalTitle] = useState('');
    const [modalBody, setModalBody] = useState('');

    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirPassword] = useState('');

    const openModal = (title: string, body:string, onClick?: Function) => {
        if (modalRef.current) {

            setModalTitle(title);
            setModalBody(body);

          modalRef.current.openModal(); // Call openModal from the Example component
        }
      };

    const handleSubmit = (event:any) => {
        event.preventDefault();
    
        try {
              if(IsValidPassword(password) == false || IsValidPassword(confirmPassword) == false) {

                openModal("Invalid Password", "Please make sure both passwords are valid."); 
                return;
              }

              if(password != confirmPassword) {

                  openModal("Passwords don't match", "Please make sure both passwords are the same."); 
                  return;
              }
            
              const data: NewPasswordModel = {
                "password": password,
                "confirmPassword": confirmPassword
              };
              
              POST('/Security/CompletePasswordReset', JSON.stringify(data))
                .then(isSuccessfull => { 
                  if(isSuccessfull) {

                      let fun = () => {
                          router.push('/', { scroll: false }); 
                        };

                      openModal("Success", "Your password has been reset successfully", fun);
                      
                  }else{
                      openModal("Error", "Something went wrong the request cannot be completed at this time")
                  }})
                  .catch(error => { console.error('Error sending data:', error);});

        } catch (error) {
          console.error('Error making API call:', error);
        }
      };

    return (
        <Container>
        <Row className="justify-content-md-center mt-5">
            <Col xs={12} md={6}>
                <h2 className="text-center mb-4">Reset Password</h2>
  
                <Form.Control
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                
                <br/>
                <Form.Control
                    type="password"
                    placeholder="Confirm Password"
                    value={confirmPassword}
                    onChange={(e) => setConfirPassword(e.target.value)}
                />

  
                <br/>
                <Button variant="primary" type="submit" className="w-100" onSubmit={handleSubmit}> Confirm </Button>

                <hr/>
                <div className="center">
                    <Link href="/" className='hyperLink'>Sign in</Link>
                </div>
            </Col>
        </Row>


        <ModalView ref={modalRef} modalTitle={modalTitle} modalBody={modalBody} />
      </Container>
    );
};
