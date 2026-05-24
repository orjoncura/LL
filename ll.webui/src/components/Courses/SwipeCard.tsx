import { motion, useMotionValue, useTransform, PanInfo } from "framer-motion";
import { useState } from "react";
import { RotateCw } from "lucide-react";
import "./SwipeCard.css";

interface SwipeCardProps {
  card: {
    id: number;
    title: string;
    description: string;
    backContent: string;
  };
  difficulty?: "easy" | "medium" | "hard";
  onSwipe: (direction: "left" | "right") => void;
  direction: number;
}

export function SwipeCard({ card, difficulty, onSwipe, direction }: SwipeCardProps) {
  const [isFlipped, setIsFlipped] = useState(false);
  const x = useMotionValue(0);
  const rotate = useTransform(x, [-200, 0, 200], [-15, 0, 15]);

  const handleDragEnd = (_: MouseEvent | TouchEvent | PointerEvent, info: PanInfo) => {
    const threshold = 100;

    if (info.offset.x > threshold) {
      onSwipe("right");
    } else if (info.offset.x < -threshold) {
      onSwipe("left");
    }
  };

  const difficultyLabels = {
    easy: "Easy",
    medium: "Medium",
    hard: "Hard",
  };

  return (
    <motion.div
      className="swipe-card-wrapper"
      style={{
        x,
        rotate,
      }}
      drag="x"
      dragConstraints={{ left: 0, right: 0 }}
      onDragEnd={handleDragEnd}
      initial={{ x: direction > 0 ? 400 : -400, opacity: 0 }}
      animate={{ x: 0, opacity: 1 }}
      exit={{ x: direction > 0 ? 400 : -400, opacity: 0 }}
      transition={{ type: "spring", stiffness: 300, damping: 30 }}
    >
      <motion.div
        className="swipe-card-inner"
        animate={{ rotateY: isFlipped ? 180 : 0 }}
        transition={{ duration: 0.6 }}
      >
        <div className="swipe-card-face swipe-card-front">
          <button
            onClick={() => setIsFlipped(true)}
            className="flip-button"
            title="Flip card"
          >
            <RotateCw className="icon-flip" />
          </button>
          {difficulty && (
            <div className={`difficulty-badge difficulty-${difficulty}`}>
              {difficultyLabels[difficulty]}
            </div>
          )}
          <h2 className="card-title">{card.title}</h2>
          <p className="card-description">{card.description}</p>
        </div>

        <div className="swipe-card-face swipe-card-back">
          <button
            onClick={() => setIsFlipped(false)}
            className="flip-button"
            title="Flip card"
          >
            <RotateCw className="icon-flip" />
          </button>
          {difficulty && (
            <div className={`difficulty-badge difficulty-${difficulty}`}>
              {difficultyLabels[difficulty]}
            </div>
          )}
          <div>
            <h3 className="back-heading">Answer:</h3>
            <p className="back-content">{card.backContent}</p>
          </div>
        </div>
      </motion.div>
    </motion.div>
  );
}
