import React, { useState, forwardRef, useImperativeHandle } from 'react';
import './FeedbackView.css';

interface FeedbackViewProps {
  title: string;
  body: string;
  text?: string;
  onClick?: () => void;
  isVisible?: boolean;
}

export interface FeedbackViewRef {
  open: () => void;
  close: () => void;
}

const FeedbackView = forwardRef<FeedbackViewRef, FeedbackViewProps>(
  ({ title, body, text, onClick, isVisible = false }, ref) => {

    const [showFeedback, setShowFeedback] = useState(isVisible);

    const open = () => {

      new Audio('../Sounds/warning-message.mp3').play();

      setShowFeedback(true)
    };

    const close = () => setShowFeedback(false);

    const confirm = () => {

      if (onClick) onClick();
      else close();
      
    };

    useImperativeHandle(ref, () => ({
      open,
      close,
    }));

    return showFeedback && (
          <div className="feedback-container fixed-bottom">
            <div className="feedback-text" >
              <div className="feedback-details">
                <h3 className="feedback-title">{title}</h3>
                {body}
              </div>
            </div>
            <button className="feedback-button mainBtn" onClick={confirm}>
              {text || "OK"}
            </button>
        </div>
    );
  }
);

export default FeedbackView;
