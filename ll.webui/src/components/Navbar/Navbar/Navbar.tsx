// components/Navbar.tsx
import React from 'react';
import Link from 'next/link';
import styles from './Navbar.module.css';

const Navbar: React.FC = () => {
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
          <Link href="/">Logout</Link>
        </li>
      </ul>
    </nav>
  );
};

export default Navbar;
