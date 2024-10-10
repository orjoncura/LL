"use client";
import React, {useState} from 'react';
import {Button, Col, Container, Form, Row} from "react-bootstrap";
import Constants from "@/scripts/Constants";
import {POST} from "@/scripts/Helpers/SecurityHelper";
import {useRouter} from "next/navigation";

export default function CreateNewAccountPage() {
    
    const [email, setEmail] = useState('');
    const router = useRouter();
    
    const handleSubmit = (event:any) => {
        event.preventDefault();

        try {
            const pattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

            if(email.trim() != '' && pattern.test(email)) {

                POST('/Security/CreateUserRequest', JSON.stringify({ "email": email}))
                    .then(data => { router.push('/', { scroll: false })})
                    .catch(error => { console.error('Error sending data:', error);});
            }
        } catch (error) {
            console.error('Error making API call:', error);

            // setResponse('An error occurred');
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
                    <Button variant="success" type="submit" className="w-100" onSubmit={handleSubmit}> Submit </Button>
                </Col>
            </Row>
        </Container>
    );
};
