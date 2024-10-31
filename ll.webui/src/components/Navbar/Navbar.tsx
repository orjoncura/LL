// components/Navbar.tsx
import React, { useState, useRef } from 'react';
import { useRouter } from 'next/navigation'
import Link from 'next/link';
import { Button } from 'react-bootstrap';
import styles from './Navbar.module.css';
import { POST, RemoveToken } from '@/scripts/Helpers/SecurityHelper'


const Navbar: React.FC = () => {

    const router = useRouter()
    const [loading, setLoading] = useState(false);
    const modalRef = useRef<any>(null);
    const [modalTitle, setModalTitle] = useState('');
    const [modalBody, setModalBody] = useState('');
    const [onModalClick, setOnModalClick] = useState<(() => void) | undefined>(undefined);

    const openModal = (title: string, body: string, onClick?: Function) => {
        if (modalRef.current) {

            setModalTitle(title);
            setModalBody(body);
            setOnModalClick(() => onClick);

            modalRef.current.openModal();
        }
    };

    const logout = (event: any) => {
        event.preventDefault();

        try {
            setLoading(true);
            POST('/Security/Logout', "")
                .then(isSuccessfull => {

                    if (isSuccessfull) {

                        RemoveToken();
                        router.push('/', { scroll: false });
                    }

                    setLoading(false);
                }).catch(e => {
                    setLoading(false);
                    openModal("Error", "The server was unable to complete your request. Please try again later.");
                });


        } catch (error) {
            console.error('Error making API call:', error);
        };
    }


  return (
    <nav className={styles.navbar}>
      <div className={styles.logo}>
        <h2>Fluente</h2>
      </div>
      <ul className={styles.links}>
        <li>
          <Link href="/Home">Home</Link>
        </li>
        <li>
          <Link href="/About">About</Link>
        </li>
        <li>
          <Link href="/Blog/1">Blog</Link>
        </li>
        <li>
          <Button variant="primary" type="submit" onClick={logout}> Logout </Button>
        </li>
      </ul>
    </nav>
  );
};

export default Navbar;
