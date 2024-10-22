"use client";
import React, { useState } from 'react';
import { Form, Button, Container, Row, Col } from 'react-bootstrap';
import { useRouter } from 'next/navigation'
import Constants from '../scripts/Constants'
import {POST} from '@/scripts/Helpers/SecurityHelper'
import {IsValidEmail} from "@/scripts/Helpers/TextHelper";
import {LoginModel} from '@/generated-client/src';
import Link from 'next/link';

import './globals.css'; 

export default function Login() {
  const router = useRouter()
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = (event:any) => {
    event.preventDefault();

    try {
          if(IsValidEmail(email) && password.trim() != '') {
            
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
              <Form.Control
                  type="password"
                  placeholder="Password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
              />

              <br/>
              <Button variant="primary" type="submit" className="w-100" onSubmit={handleSubmit}> Login </Button>

              <hr/>
              <div className="center">
                  <Link href="/Security/ResetPassword" className='hyperLink'>Forgotten password?</Link>
              </div>

              <br/>
              <Link href="/Security/CreateUserRequest" className="w-100 button-link btn btn-success">
                  Create new account
              </Link>
          </Col>
      </Row>
    </Container>
  );
}