import React from 'react';
import './SpinnerOverlay.css'; // Import CSS for styling

const SpinnerOverlay = () => {
  return (
    <div className="spinner-overlay">
      <div className="loader">
        <span></span>
        <span></span>
        <span></span>
        <span></span>
        <span></span>
        <span></span>
      </div>
    </div>
  );
};

export default SpinnerOverlay;
