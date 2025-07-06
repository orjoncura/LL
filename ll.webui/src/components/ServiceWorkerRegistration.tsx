"use client";

import { useState, useEffect } from 'react';

export default function ServiceWorkerRegistration() {

    const [granted, setGranted] = useState<boolean>(false);  

    useEffect(() => {
        if ('serviceWorker' in navigator) {
        navigator.serviceWorker
            .register('/service-worker.js')
            .then((reg) => handleAskPermission())
            .catch((err) => setGranted(false));
        }
    }, []);

    async function requestNotificationPermission(): Promise<NotificationPermission> {
        if (!('Notification' in window)) {
            console.warn('This browser does not support notifications.');
            return 'denied';
        }

        const permission = await Notification.requestPermission();
        console.log('🔔 Notification permission:', permission);
        return permission;
    }

    const handleAskPermission = async () => {
        const permission = await requestNotificationPermission();

        if (permission === 'granted') {
            setGranted(true)
        }else{
            setGranted(false)
        }
    };

    return (
        <div>
            {granted == false && <button onClick={handleAskPermission} style={{width:"100%"}}>Enable Notifications</button>}
        </div>
    );
}