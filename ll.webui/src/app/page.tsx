"use client";
import React, {useState, useRef} from 'react';
import { Form, Button, Container, Row, Col } from 'react-bootstrap';
import { useRouter } from 'next/navigation'
import Constants from '../scripts/Constants'
import {POST} from '@/scripts/Helpers/SecurityHelper'
import {IsValidEmail} from "@/scripts/Helpers/TextHelper";
import {LoginModel, TokenViewModel} from '@/generated-client/src';
import Link from 'next/link';
import ModalView from '../components/Modal/ModalView';
import SpinnerOverlay from '../components/Spinner/SpinnerOverlay';
import './globals.css'; 

export default function Login() {

    const router = useRouter()
    const [loading, setLoading] = useState(false);
    const modalRef = useRef<any>(null); 
    const [modalTitle, setModalTitle] = useState('');
    const [modalBody, setModalBody] = useState('');
    const [onModalClick, setOnModalClick] = useState<(() => void) | undefined>(undefined);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const openModal = (title: string, body:string, onClick?: Function) => {
        if (modalRef.current) {

            setModalTitle(title);
            setModalBody(body);
            setOnModalClick(() => onClick); 

            modalRef.current.openModal(); // Call openModal from the Example component
        }
    };

    const handleSubmit = (event: any) => {
        event.preventDefault();
  
        try {
            if (IsValidEmail(email) && password.trim() != '') {
  
                const data: LoginModel = {
                    "email": email,
                    "password": password
                };
  
                setLoading(true);
                POST('/Security/Authenticate', JSON.stringify(data))
                    .then((tokenModel: TokenViewModel) => {

                        console.log("tokenModel:", tokenModel);

                        if (tokenModel != null) {
  
                            router.push('/Home', { scroll: false });
  
                        } else {
                            openModal("Error", "Something went wrong the request cannot be completed at this time");
                        }
  
                        setLoading(false);
                    }).catch(e => {
                        setLoading(false);
                        openModal("Error", "The server was unable to complete your request. Please try again later.");
                    });
            }
  
        } catch (error) {
            console.error('Error making API call:', error);
        };
    }

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
              <Form.Control
                  type="password"
                  placeholder="Password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
              />

              <br/>
              <Button variant="primary" type="submit" className="w-100" onClick={handleSubmit}> Login </Button>

              <hr/>
              <div className="center">
                  <Link href="/Security/ResetPassword" className='hyperLink'>Forgotten password?</Link>
              </div>

              <br/>
              <Link href="/Security/RegisterUser" className="w-100 button-link btn btn-success">
                  Create new account
              </Link>
          </Col>
      </Row>

      {loading && <SpinnerOverlay />}
      <ModalView ref={modalRef} modalTitle={modalTitle} modalBody={modalBody} onClick={onModalClick} />
    </Container>
  );
}