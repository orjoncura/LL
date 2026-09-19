'use client';

import { useEffect, useState } from 'react';
import { GetToken } from '@/utils/Security/AuthManager'
import ServiceWorkerRegistration from './System/ServiceWorkerRegistration';
import { SyncOfflineQueue } from '@/utils/Security/httpClient';

export default function ProtectedLayout({ children }: { children: React.ReactNode }) {
  const [hasMounted, setHasMounted] = useState(false);

  useEffect(() => {
    setHasMounted(true);

    const handleOnline = () => { void SyncOfflineQueue(); };
    window.addEventListener('online', handleOnline);
    void SyncOfflineQueue();

    return () => window.removeEventListener('online', handleOnline);
  }, []);

  if (!hasMounted) return null; 

  const currentPath = window.location.pathname;
  // Allow home and all auth flows (register, confirm email, reset password, etc.)
  const isPublicPage = currentPath === '/' || currentPath.startsWith('/Security');

  if (!GetToken() && !isPublicPage) {
    window.location.replace('/');
    return null;
  }

  return <><ServiceWorkerRegistration/>{children}</>;
}
