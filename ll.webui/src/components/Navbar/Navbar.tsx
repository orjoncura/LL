// components/Navbar.tsx
import React from 'react';
import { useRouter } from 'next/navigation'
import Link from 'next/link';
import styles from './Navbar.module.css';
import { LogOut } from '@/utils/Controllers/SecurityController'
import { RemoveToken } from '@/utils/Security/AuthManager'

const Navbar: React.FC = () => {

    const router = useRouter();

    const logout = (event: any) => {
        event.preventDefault();

        try {
            LogOut()
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
      <div className={styles.links}>
        <h2>
          <Link className='mainTxt' href="/Course/Create">Fluente</Link>
        </h2>
      </div>
      <ul className={styles.links}>
        <li>
          <Link className='mainTxt' href="/Course/CourseList" prefetch={true}>Courses</Link>
        </li>
        <li>
          <Link className='mainTxt' href="/Profile/Overview">Profile</Link>
        </li>
        <li>
          <button type="button" onClick={logout} className={`${styles.linkButton} mainTxt`}>Logout</button>
        </li>
      </ul>
    </nav>
  );
};

export default Navbar;
