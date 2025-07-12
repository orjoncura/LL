'use client';

import { useEffect, useState } from 'react';
import { GetToken } from '@/utils/Security/AuthManager'
import ServiceWorkerRegistration from '../components/ServiceWorkerRegistration';

export default function ProtectedLayout({ children }: { children: React.ReactNode }) {
  const [hasMounted, setHasMounted] = useState(false);

  useEffect(() => { setHasMounted(true); }, []);

  if (!hasMounted) return null; 

  const currentPath = window.location.pathname;
  const isPublicPage = /^\/(Security)?$/.test(currentPath);

  if (!GetToken() && !isPublicPage) {
    window.location.replace('/');
    return null;
  }

  return <><ServiceWorkerRegistration/>{children}</>;
}
