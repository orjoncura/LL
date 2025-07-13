"use client";

import { useState, useEffect } from 'react';

interface BeforeInstallPromptEvent extends Event {
  prompt: () => Promise<void>;
  userChoice: Promise<{ outcome: 'accepted' | 'dismissed'; platform: string }>;
}

export default function ServiceWorkerRegistration() {

    const [deferredPrompt, setDeferredPrompt] = useState<BeforeInstallPromptEvent | null>(null);
    const [showSafariInstructions, setShowSafariInstructions] = useState(false);
    const [showNotifications, setShowNotifications] = useState<boolean>(false);
    const [isInstallable, setIsInstallable] = useState<boolean>(false);
    const [isMacOS, setIsMacOS] = useState<boolean>(false);

    useEffect(() => {
        const userAgent = navigator.userAgent.toLowerCase();

        let granted:boolean = (typeof window !== 'undefined' && 'Notification' in window) 
            ? (Notification.permission === 'granted')
            : false;
        
        let isSafari:boolean = (/safari/.test(userAgent) && !/chrome/.test(userAgent));
        setIsMacOS(/Macintosh/i.test(navigator.userAgent));

        if ('serviceWorker' in navigator) {
            navigator.serviceWorker
            .register('/service-worker.js');
        }

        if ('beforeinstallprompt' in window) {
            const handler = (e: Event) => {
                e.preventDefault();
                setDeferredPrompt(e as BeforeInstallPromptEvent);
                setIsInstallable(true);
            };
            window.addEventListener('beforeinstallprompt', handler);

            return () => window.removeEventListener('beforeinstallprompt', handler);
        } else if (isSafari) {
            const isInStandaloneMode = 'standalone' in window.navigator && window.navigator.standalone;

            setShowSafariInstructions(isInStandaloneMode == false);
        }

        setShowNotifications(isInstallable && !showSafariInstructions && !granted);
    }, []);

    const handleInstallClick = async () => {
        if (deferredPrompt) {
        deferredPrompt.prompt();
        const choice = await deferredPrompt.userChoice;
        if (choice.outcome === 'accepted') {
            console.log('App installed');
            requestNotificationPermission();
        }
        }
    };

    const requestNotificationPermission = async () => {

        try {
            const permission = await Notification.requestPermission();
            setShowNotifications(permission != 'granted');
        } catch (err) {
            console.error('Error requesting notification permission:', err);
        }
    };

    return (
        <div>
            {isInstallable && (<button onClick={handleInstallClick} style={{width:"100%"}}> Install App & Enable Notifications </button> )}

            {showSafariInstructions && (
                <div style={{ padding: '1rem', border: '1px solid gray', borderRadius: '8px' }}>
                <strong>Safari Installation Instructions:</strong>
                <p>
                    To install this app, click the <b>Share</b> button in your browser and select
                    <b> “Add to Home Screen”</b>.
                </p>
                {isMacOS && (
                    <p>
                    On macOS, you can also use the “Add to Dock” option in the address bar.
                    </p>
                )}
                <button className='btn btn-primary' onClick={requestNotificationPermission}>
                    Enable Notifications
                </button>
                </div>
            )}

            {showNotifications && (
                <button onClick={requestNotificationPermission} className='btn btn-primary' style={{width:"100%", borderRadius:"0"}}> Enable Notifications </button> )}

        </div>
    );
}