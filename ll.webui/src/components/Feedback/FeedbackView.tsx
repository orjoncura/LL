import React, { useState, forwardRef, useImperativeHandle } from 'react';
import './FeedbackView.css';

interface FeedbackViewProps {
  title: string;
  body: string;
  text?: string;
  onClick?: () => void;
  isVisible?: boolean;
  showCloseBtn?: boolean;
}

export interface FeedbackViewRef {
  open: () => void;
  close: () => void;
}

const FeedbackView = forwardRef<FeedbackViewRef, FeedbackViewProps>(
  ({ title, body, text, onClick, isVisible = false, showCloseBtn = true }, ref) => {

    const [showFeedback, setShowFeedback] = useState(isVisible);

    const open = () => {

      new Audio('/Sounds/warning-message.mp3').play();

      setShowFeedback(true)
    };

    const confirm = () => { if(onClick) onClick(); close()};
    const close = () => setShowFeedback(false);

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
            {onClick && <button className="mainBtn feedback-button" onClick={confirm}>
              {text}
            </button>}
            {showCloseBtn && <button className="mainBtn feedback-button" onClick={close}>
              Close
            </button>}
        </div>
    );
  }
);

export default FeedbackView;
