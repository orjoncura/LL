import type { Metadata } from "next";
import { Inter } from "next/font/google";
import {getEnv} from '@/utils/Models/EnvironmentVariables';
import ProtectedLayout from '../components/ProtectedLayout';

import "./globals.css";
import 'bootstrap/dist/css/bootstrap.min.css';

const inter = Inter({ subsets: ["latin"]});

export const metadata: Metadata = {
    title: getEnv().Application_Name || '',
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
          <ProtectedLayout children={children} />
        </body>
      </html>
  );
}
