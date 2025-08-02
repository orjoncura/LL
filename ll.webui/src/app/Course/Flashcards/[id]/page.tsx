"use client"
 
import { useState, useEffect } from 'react';
import { GET } from '@/utils/Security/httpClient'
import { WordViewModel } from '@/utils/Models/models';
import { useParams } from 'next/navigation'
import { useRouter } from 'next/navigation';

import Navbar from '@/components/Navbar/Navbar';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import Flashcards from '@/components/Courses/Flashcards';

export default function FlashcardsPage()  {

    const [wordViewModels, setWordViewModels] = useState<WordViewModel[]>([]);  
    const [selectedCourse, setSelectedCourse] = useState<string>("");    
    const [showCourse, setShowCourse] = useState(false);
    const [loading, setLoading] = useState(false); 
    const params = useParams<{ id: string; }>()
    const router = useRouter();

    let hasFetchedData = false;
    useEffect(() => {

        if(hasFetchedData == false){
            hasFetchedData = true;

            setLoading(true);

            var text = localStorage.getItem("SelectedCourse") || "";
            if(text == "") router.push(`/Course/CourseList`);

            setSelectedCourse(text);

            GET('/Course/GetCourseWords?moduleId=' + params.id)
            .then((words: WordViewModel[]) => {

                if(words == null || words.length == 0)
                return;

                const unique = words.filter((val, id, array) => {
                return array.indexOf(val) == id;  
                });

                setWordViewModels(unique)
                setShowCourse(words.length > 0)

            }).finally(() => setLoading(false));
        }
    }, []);

    return (
        <div>
            <Navbar /> 

            {showCourse && (<Flashcards text={selectedCourse} words={wordViewModels} moduleId={params.id} onDone={() => router.back()}  />)}
            {loading && <SpinnerOverlay />}
        </div>
    );
}