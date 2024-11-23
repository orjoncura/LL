// components/Navbar.tsx
import React from 'react';
import { useRouter } from 'next/navigation'
import Link from 'next/link';
import styles from './Navbar.module.css';
import { POST, RemoveToken } from '@/scripts/Helpers/SecurityHelper'


const Navbar: React.FC = () => {

    const router = useRouter()
    const logout = (event: any) => {
        event.preventDefault();

        try {
            POST('/Security/Logout', "")
                .then(isSuccessfull => {

                    if (isSuccessfull) {

                        RemoveToken();
                        router.push('/', { scroll: false });
                    }

                }).catch(e => {
                    console.log("The server was unable to complete your request. Please try again later.");
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
          <Link href="/Seminar/CreateSeminar">Home</Link>
        </li>
        <li>
          <Link href="/About">About</Link>
        </li>
        <li>
          <Link href="/Blog/1">Blog</Link>
        </li>
        <li>
          <button type="button" onClick={logout} className={styles.linkButton}>Logout</button>
        </li>
      </ul>
    </nav>
  );
};

export default Navbar;
