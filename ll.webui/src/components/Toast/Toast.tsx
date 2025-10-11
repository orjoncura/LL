import { useState } from "react";
import Button from 'react-bootstrap/Button';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import Toast from 'react-bootstrap/Toast';
import "./Toast.css";

interface ToastItem {
  id: number;
  message: string;
}

export function useToast() {
  const [toasts, setToasts] = useState<ToastItem[]>([]);

  const showToast = (message: string) => {
    const id = Date.now();
    setToasts((prev) => [...prev, { id, message }]);

    // Auto-remove after 3 seconds
    setTimeout(() => {
      setToasts((prev) => prev.filter((t) => t.id !== id));
    }, 8000);
  };


  const ToastContainer = () => (
    <div className="position-fixed top-0 end-0 p-3" style={{ zIndex: 1080 }}>
      {toasts.map((toast) => (
        <Row key={toast.id}>
          <Toast>
            <Toast.Body>{toast.message}</Toast.Body>
          </Toast>
        </Row>
      ))}
    </div>
  );

  return { showToast, ToastContainer };
}
