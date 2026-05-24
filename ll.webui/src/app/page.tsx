"use client"

import React, {useState, useRef, useEffect} from 'react';
import { Authenticate} from '@/utils/Controllers/SecurityController'
import { StoreToken } from '@/utils/Security/AuthManager'
import { getEnv } from '@/utils/Models/EnvironmentVariables';
import { IsValidEmail, IsValidPassword } from "@/utils/Security/Validators";
import { TokenViewModel } from '@/utils/Models/models';
import { useRouter } from 'next/navigation'
import './LoginPage.css';
import Link from 'next/link';

import FeedbackView from '../components/Feedback/FeedbackView';
import SpinnerOverlay from '../components/Spinner/SpinnerOverlay';

export default function Login() {
    const router = useRouter()
    const [loading, setLoading] = useState(false);
    const feedbackViewRef = useRef<any>(null); 
    const [fbTitle, setfbhTitle] = useState('');
    const [fbBody, setfbhBody] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [onFeedBackViewClick, setOnFeedBackViewClick] = useState<(() => void) | undefined>(undefined);

    useEffect(() => { console.log("Version:" + getEnv().Version)}, []);

    const showFeedback = (title: string, body:string, onClick?: Function) => {
        if (feedbackViewRef.current) {

            setfbhTitle(title);
            setfbhBody(body);
            setOnFeedBackViewClick(() => onClick); 

            feedbackViewRef.current.open();
        }
    };

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();

    try {
        if (IsValidEmail(email) == false) {

            showFeedback("Error", "Invalid email address format.");
            return;
        }

        if (IsValidPassword(password) == false) {

            showFeedback("Error", "Invalid password format.");
            return;
        }

        setLoading(true);
        Authenticate(email, password)
            .then((tokenModel: TokenViewModel) => {

                if (tokenModel.token != null && tokenModel.token.length > 1) {

                    StoreToken(tokenModel.token);
                    router.push('/Course/Create', { scroll: false });

                } else {
                    showFeedback("Error", "It looks like the username or password you entered doesn't match our records." 
                    + " Please double - check and try again.");
                }

                setLoading(false);
            }).catch(e => {
                setLoading(false);
                showFeedback("Error", "The server was unable to complete your request. Please try again later.");
            });
        

    } catch (error) {
        console.error('Error making API call:', error);
    };
  };

  const handleCreateAccount = () => {
    console.log('Create account clicked');
  };

  return (
    <div className="login-container">
      <div className="login-wrapper">
        <div className="login-card">
          <h1 className="login-title">Welcome Back</h1>
          <p className="login-subtitle">Sign in to your account</p>

          <form onSubmit={handleLogin} className="login-form">
            <div className="form-group">
              <label htmlFor="email" className="form-label">
                Email Address
              </label>
              <input
                id="email"
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="form-input"
                placeholder="you@example.com"
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="password" className="form-label">
                Password
              </label>
              <input
                id="password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="form-input"
                placeholder="Enter your password"
                required
              />
            </div>

            <div className="forgot-password-wrapper">
                <Link href="/Security/ResetPassword" className='mainLink'>Forgot password?</Link>
            </div>

            <button type="submit" className="mainBtn">
              Sign In
            </button>
          </form>

          <div className="create-account-section">
            <p className="create-account-text">Don't have an account?</p>
            <Link href="/Security/RegisterUser" className="mainBtn w-100 button-link">
                  Create new account
            </Link>
          </div>
          
        </div>
      </div>
    {loading && <SpinnerOverlay />}
    <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody} onClick={onFeedBackViewClick} />
    </div>
  );
}