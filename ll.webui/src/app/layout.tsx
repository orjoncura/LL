import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import 'bootstrap/dist/css/bootstrap.min.css';
import Constants from '../scripts/Constants'

const inter = Inter({ subsets: ["latin"]});

export const metadata: Metadata = {
    title: Constants().ApplicationName,
  description: "Become your own teacher",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
      <html lang="en" >
        <body style={{ background: 'black', color: 'white' }} className={inter.className}>
          {children} 
        </body>
      </html>
  );
}
