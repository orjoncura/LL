import { createContext, useContext, useState, ReactNode } from 'react';
import { useRouter } from 'next/router';

interface AuthContextType {
  isAuthenticated: boolean;
  login: (email: string, password: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const router = useRouter();

  const login = (email: string, password: string) => {
    // Replace this with real authentication logic
    if (email === 'user@example.com' && password === 'password123') {
      setIsAuthenticated(true);
      router.push('/dashboard'); // Redirect after login
    } else {
      alert('Invalid credentials');
    }
  };

  const logout = () => {
    setIsAuthenticated(false);
    router.push('/login'); // Redirect after logout
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
