import { useState } from "react";
import { SwipeCard } from "./SwipeCard";
import { ThumbsDown, Minus, ThumbsUp, ChevronLeft, ChevronRight } from "lucide-react";
import { AnimatePresence } from "framer-motion";
import "./Flashcards.css";

export interface meaningShort {
  id: string;
  value: string;
}

export interface WordViewModel {
  id: string;
  name: string;
  translation?: string;
  language?: string;
  meanings?: meaningShort[];
  importanceRatingId: number;
}

interface FlashcardViewerProps {
  words: WordViewModel[];
}

export function FlashcardViewer({ words }: FlashcardViewerProps) {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [direction, setDirection] = useState(0);
  const [ratings, setRatings] = useState<Record<string, "easy" | "medium" | "hard">>({});

  const handleSwipe = (swipeDirection: "left" | "right") => {
    if (swipeDirection === "right" && currentIndex < words.length - 1) {
      setDirection(1);
      setCurrentIndex((prev) => prev + 1);
    } else if (swipeDirection === "left" && currentIndex > 0) {
      setDirection(-1);
      setCurrentIndex((prev) => prev - 1);
    }
  };

  const handleRating = (difficulty: "easy" | "medium" | "hard") => {
    setRatings((prev) => ({
      ...prev,
      [words[currentIndex].id]: difficulty,
    }));
  };

  const navigate = (dir: "prev" | "next") => {
    if (dir === "next" && currentIndex < words.length - 1) {
      setDirection(1);
      setCurrentIndex((prev) => prev + 1);
    } else if (dir === "prev" && currentIndex > 0) {
      setDirection(-1);
      setCurrentIndex((prev) => prev - 1);
    }
  };

  const mapImportanceToRating = (importanceId: number): "easy" | "medium" | "hard" | undefined => {
    if (importanceId === 1) return "easy";
    if (importanceId === 2) return "medium";
    if (importanceId === 3) return "hard";
    return undefined;
  };

  if (words.length === 0) {
    return (
      <div className="flashcard-viewer">
        <div className="no-words">
          <p className="no-words-text">No flashcards available</p>
        </div>
      </div>
    );
  }

  const currentWord = words[currentIndex];
  const cardData = {
    id: parseInt(currentWord.id) || 0,
    title: currentWord.name,
    description: currentWord.language || "",
    backContent: currentWord.translation || "No translation available",
  };

  const currentRating = ratings[currentWord.id] || mapImportanceToRating(currentWord.importanceRatingId);

  return (
    <div className="flashcard-viewer">
      <div className="flashcard-instructions">
        <p className="flashcard-instructions-text">
          Swipe left for previous • Swipe right for next • Click 🔄 to flip
        </p>
        <p className="flashcard-counter">
          Card {currentIndex + 1} of {words.length}
        </p>
      </div>

      <div className="flashcard-cards-container">
        <AnimatePresence initial={false} custom={direction}>
          <SwipeCard
            key={currentWord.id}
            card={cardData}
            difficulty={currentRating}
            onSwipe={handleSwipe}
            direction={direction}
          />
        </AnimatePresence>

        <button
          onClick={() => navigate("prev")}
          disabled={currentIndex === 0}
          className="flashcard-nav-button flashcard-nav-button-left"
        >
          <ChevronLeft className="flashcard-icon-nav" />
        </button>

        <button
          onClick={() => navigate("next")}
          disabled={currentIndex === words.length - 1}
          className="flashcard-nav-button flashcard-nav-button-right"
        >
          <ChevronRight className="flashcard-icon-nav" />
        </button>
      </div>

      <div className="flashcard-rating-section">
        <p className="flashcard-rating-label">Rate this card:</p>
        <div className="flashcard-rating-buttons">
          <button
            onClick={() => handleRating("hard")}
            className={`flashcard-rating-button ${
              currentRating === "hard" ? "flashcard-active-hard" : ""
            }`}
            title="Hard"
          >
            <ThumbsDown className="flashcard-icon-hard" />
            <span className="flashcard-rating-button-label">Hard</span>
          </button>
          <button
            onClick={() => handleRating("medium")}
            className={`flashcard-rating-button ${
              currentRating === "medium" ? "flashcard-active-medium" : ""
            }`}
            title="Medium"
          >
            <Minus className="flashcard-icon-medium" />
            <span className="flashcard-rating-button-label">Medium</span>
          </button>
          <button
            onClick={() => handleRating("easy")}
            className={`flashcard-rating-button ${
              currentRating === "easy" ? "flashcard-active-easy" : ""
            }`}
            title="Easy"
          >
            <ThumbsUp className="flashcard-icon-easy" />
            <span className="flashcard-rating-button-label">Easy</span>
          </button>
        </div>
      </div>
    </div>
  );
}
