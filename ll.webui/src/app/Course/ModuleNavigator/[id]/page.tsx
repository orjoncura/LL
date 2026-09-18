"use client";

import { useState, useEffect } from "react";
import { motion } from "framer-motion";
import { GetModulesByCourseId } from '@/utils/Controllers/CourseController'
import { ModuleViewModel} from '@/utils/Models/models';
import { useParams } from 'next/navigation'
import { useRouter } from 'next/navigation';
import { CheckCircle2, Circle, Play, BookOpen, Dumbbell, ChevronRight } from 'lucide-react';

import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import Navbar from '@/components/Navbar/Navbar';

import './page.css';


export default function ModuleNavigator() {
  const [modules, setModules] = useState<ModuleViewModel[]>([]);
  const [loading, setLoading] = useState(false);
  const params = useParams<{ id: string; }>()
  const router = useRouter();
  const completedCount = modules.filter(m => m.completed).length;
  const totalCount = modules.length;
  const progressPercentage = (completedCount / totalCount) * 100;
  const nextModule = modules.find(m => !m.completed);

  let hasFetchedData = false;

  useEffect(() => {

    if(hasFetchedData == false){
      hasFetchedData = true;

      setLoading(true);

      GetModulesByCourseId(params.id)
      .then((moduleViewModel: ModuleViewModel[]) => {

        if(moduleViewModel == null || moduleViewModel.length == 0)
          return;

        const unique = moduleViewModel.filter((val, id, array) => {
          return array.indexOf(val) == id;  
        });

        setModules(unique)

      }).finally(() => setLoading(false));
    }
  }, []);

  const navigateToModule = async (mod: ModuleViewModel) => {
      
    if(mod.typeId == 1)
      router.push(`/Course/Flashcards/${mod.id}`)

    if(mod.typeId == 2)
      router.push(`/Course/Exercises/${mod.id}`)

    if(mod.typeId == 3)
      router.push(`/Course/Multiselect/${mod.id}`)
  };


  return (
    <div>
      <Navbar/> 
      <div className="app-container">
        <div className="app-content">
          {/* Header */}
          <div className="header">
            <h1 className="title">Learning Path</h1>
            <p className="subtitle">Master the fundamentals through structured practice</p>

            {/* Progress Overview */}
            <div className="progress-card">
              <div className="progress-header">
                <span className="progress-label">Overall Progress</span>
                <span className="progress-count">{completedCount} of {totalCount} completed</span>
              </div>
              <div className="progress-bar-container">
                <div
                  className="progress-bar-fill"
                  style={{ width: `${progressPercentage}%` }}
                />
              </div>
            </div>
          </div>

          {/* Module List */}
          <div className="module-list">
            {modules.map((module) => {
              const Icon = module.typeId == 1 ? BookOpen : Dumbbell;
              const label = module.typeId == 1 ? 'Flashcards' : 'Exercises';

              return (
                <div
                  key={module.id}
                  className={`module-card ${module.completed ? 'completed' : 'incomplete'}`}
                >
                  <div className="module-content">
                    {/* Status Icon */}
                    <div className={`status-icon ${module.completed ? 'completed' : 'incomplete'}`}>
                      {module.completed ? (
                        <CheckCircle2 size={24} color="white" />
                      ) : (
                        <Circle size={24} color="#9ca3af" />
                      )}
                    </div>

                    {/* Content */}
                    <div className="module-info">
                      <div className="module-title-row">
                        <Icon size={16} color={module.completed ? 'var(--color-primary)' : 'var(--color-text-disabled)'} />
                        <h3 className={`module-title ${module.completed ? 'completed' : 'incomplete'}`}>
                          {label}: {module.title}
                        </h3>
                      </div>
                      <p className="module-status">
                        {module.completed ? 'Completed' : 'Not started'}
                      </p>
                    </div>

                    {/* Action Button */}
                    <button
                      onClick={() => navigateToModule(module)}
                      disabled={module.completed}
                      className={`action-button ${module.completed ? 'completed' : 'incomplete'}`}
                    >
                      {module.completed ? (
                        <>
                          <CheckCircle2 size={16} />
                          Done
                        </>
                      ) : (
                        <>
                          <Play size={16} />
                          Start
                        </>
                      )}
                    </button>
                  </div>

                  {/* Completion indicator line */}
                  {module.completed && <div className="completion-line" />}
                </div>
              );
            })}
          </div>

          {/* Next Step Suggestion */}
          {completedCount < totalCount && nextModule && (
            <div className="continue-card">
              <div className="continue-content">
                <div>
                  <h3 className="continue-title">Continue Learning</h3>
                  <p className="continue-subtitle">
                    {nextModule.type === 'flashcard' ? 'Flashcards' : 'Exercises'}: Part {nextModule.part}
                  </p>
                </div>
                <button
                  onClick={() => navigateToModule(nextModule.id)}
                  className="continue-button"
                >
                  Continue
                  <ChevronRight size={16} />
                </button>
              </div>
            </div>
          )}

          {/* Completion Message */}
          {completedCount === totalCount && (
            <div className="completion-card">
              <div className="completion-icon">
                <CheckCircle2 size={40} />
              </div>
              <h3 className="completion-title">Congratulations!</h3>
              <p className="completion-message">You've completed all modules in this learning path.</p>
            </div>
          )}
        </div>
      </div>

        {loading && <SpinnerOverlay />}
    </div>
  );
}
