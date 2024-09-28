"use client";
import React, { useState } from 'react';
import { Form, Button, Container, Row, Col } from 'react-bootstrap';
import { useRouter } from 'next/navigation'
import Constants from '../scripts/Constants'
import {POST} from '../scripts/Helpers/SecurityHelper'
import {LoginModel} from "@/scripts/Models/LoginModel";

export default function Login() {
  const router = useRouter()
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = (event:any) => {
    event.preventDefault();
    console.log('Login attempted with:', { email, password });

    try {
          if(email.trim() != '' && password.trim() != '') {
            
            const data: LoginModel = {
              "email": email,
              "password": password
            };
            
            POST('/Security/Authenticate', JSON.stringify(data))
                .then(data => { router.push('/Home', { scroll: false })})
                .catch(error => { console.error('Error sending data:', error);});
      }
    } catch (error) {
      console.error('Error making API call:', error);
      
      // setResponse('An error occurred');
    }
    //
  };

  return (
    <Container>
      <Row className="justify-content-md-center mt-5">
        <Col xs={12} md={6}>
          <h2 className="text-center mb-4">{Constants().ApplicationName}</h2>
          <Form onSubmit={handleSubmit}>
            <Form.Group className="mb-3" controlId="formBasicEmail">
              <Form.Label>Email address</Form.Label>
              <Form.Control
                type="email"
                placeholder="Enter email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </Form.Group>

            <Form.Group className="mb-3" controlId="formBasicPassword">
              <Form.Label>Password</Form.Label>
              <Form.Control
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </Form.Group>

            <Button variant="primary" type="submit" className="w-100">
              Login
            </Button>
          </Form>
        </Col>
      </Row>
    </Container>
  );
}