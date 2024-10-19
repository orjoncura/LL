"use client";
import React, {useEffect} from 'react';
import { useSearchParams } from "react-router-dom";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faSmile, faFaceFrown } from '@fortawesome/free-solid-svg-icons';
import {Container} from "react-bootstrap";
import {POST} from "@/scripts/Helpers/SecurityHelper";
import Link from 'next/link';

export default function CompleteResetPassword() {

    let isVerified:boolean = false;  

    useEffect(() => {

        const searchParams = new URLSearchParams(window.location.search);
        let token = searchParams.get('token');

        if(token != null && token.length > 1) {

            POST('/Security/CompleteResetPassword', JSON.stringify(token))
                .then(isSuccessfull => isVerified = isSuccessfull)
                .catch(error => { console.error('Error sending data:', error);});
        }
      }, []);
    
    return (
        <Container>
             <div className="d-flex justify-content-center align-items-center" style={{ height: '100vh' }}>
                <div className="p-4 rounded">
                    {isVerified ? 
                    (<div style={{ color: 'Green', display: '' }}>
                        <div style={{ textAlign: 'center', fontSize: '200px'}}>
                            <FontAwesomeIcon icon={faSmile} />
                            <br /><br />
                            <label style={{ marginRight: '5px' }}>Your password has been reset successfully! Please </label>
                            <Link href="/">Sign in</Link>
                        </div>
                    </div>)
                    : (<div style={{ color: 'Red' }}>
                        <div style={{ textAlign: 'center', fontSize: '200px'}}>
                            <FontAwesomeIcon icon={faFaceFrown}/>
                        </div>
                        <br/>
                        <label>Error! This request is invalid</label>
                    </div>)}
                </div>
            </div>
        </Container>
    );
};
