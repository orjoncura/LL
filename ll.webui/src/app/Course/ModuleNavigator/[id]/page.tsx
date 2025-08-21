"use client";

import { useState, useEffect } from "react";
import { motion } from "framer-motion";
import { GET } from '@/utils/Security/httpClient'
import { ModuleViewModel} from '@/utils/Models/models';
import { useParams } from 'next/navigation'
import { useRouter } from 'next/navigation';

import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import Navbar from '@/components/Navbar/Navbar';

import './page.css'; 

export default function ModuleNavigator()  {
  const [modules, setModules] = useState<ModuleViewModel[]>([]);
  const [loading, setLoading] = useState(false);
  const params = useParams<{ id: string; }>()
  const router = useRouter();

  let hasFetchedData = false;

  useEffect(() => {

    if(hasFetchedData == false){
      hasFetchedData = true;

      setLoading(true);

      GET('/Course/GetModulesByCourseId?courseId=' + params.id)
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
      <Navbar /> 

      <div className="module-container">
        {modules.map((mod, idx) => {
          // const Icon = iconMap[mod.type];
          const isActive = mod.unlocked;
          const isCompleted = mod.completed;

          return (
            <motion.div
              key={mod.id + idx}
              className={`module-card ${isCompleted ? 'completed' : ''} ${!isActive ? 'locked' : ''}`}
              whileHover={{ scale: isActive ? 1.02 : 1 }}
              initial={{ opacity: 0, y: 10 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: idx * 0.1 }}
            >
              <div className="module-info">
                <div className={`module-icon ${isCompleted ? 'icon-done' : ''}`}>
                </div>
                <div className="module-text">
                  <div className="module-title">{mod.title}</div>
                </div>
              </div>
              {isActive && (
                <button className="module-btn" onClick={() => navigateToModule(mod)}>
                  Start
                </button>
              )}
            </motion.div>
          );
        })}

        {loading && <SpinnerOverlay />}
      </div>
    </div>
  );
}