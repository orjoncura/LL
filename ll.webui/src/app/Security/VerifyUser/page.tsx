"use client";
import React, {useEffect} from 'react';
import { useSearchParams } from "react-router-dom";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faSmile, faFaceFrown } from '@fortawesome/free-solid-svg-icons';
import {Container} from "react-bootstrap";
import {POST} from "@/scripts/Helpers/SecurityHelper";
import Link from 'next/link';

export default function VerifyUser() {

    let isVerified:boolean = false;  

    useEffect(() => {

        const searchParams = new URLSearchParams(window.location.search);
        let token = searchParams.get('token');

        if(token != null && token.length > 1) {

            POST('/Security/VerifyUser', JSON.stringify(token))
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
                            <FontAwesomeIcon icon={faSmile}/>
                        </div>
                        <br /><br />
                        <label style={{ marginRight: '5px' }}>You account has been verified! Please </label>
                        <Link href="/">sign in</Link>
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
