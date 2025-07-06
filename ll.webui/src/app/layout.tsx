import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import 'bootstrap/dist/css/bootstrap.min.css';
import {getEnv} from '@/scripts/Helpers/EnvironmentVariables';
import ServiceWorkerRegistration from '../components/ServiceWorkerRegistration';

const applicationName = getEnv().Application_Name || '';

const inter = Inter({ subsets: ["latin"]});

export const metadata: Metadata = {
    title: applicationName,
  description: "Become your own teacher",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
      <html lang="en" >
        <link rel="manifest" href="/manifest.json" />
        <body style={{ background: '#fff0f6', color: '#d63384' }}>
          <ServiceWorkerRegistration />
          {children} 
        </body>
      </html>
  );
}
