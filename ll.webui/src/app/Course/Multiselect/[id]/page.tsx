"use client"
 
import Navbar from '@/components/Navbar/Navbar';
import SpinnerOverlay from '@/components/Spinner/SpinnerOverlay';
import Multiselect from '@/components/Courses/Multiselect';
import FeedbackView from '@/components/Feedback/FeedbackView';

import { useState, useEffect, useRef } from 'react';
import { MarkModuleAsComplete, GetCourseWords } from '@/utils/Controllers/CourseController'
import { WordViewModel } from '@/utils/Models/models';
import { useRouter, useParams } from 'next/navigation';

export default function MultiselectPage()  {
  
    const feedbackViewRef = useRef<any>(null); 
    const [wordViewModels, setWordViewModels] = useState<WordViewModel[]>([]);  
    const [showCourse, setShowCourse] = useState(false);
    const [loading, setLoading] = useState(false); 
    const [fbTitle, setfbhTitle] = useState('');
    const [fbBody, setfbhBody] = useState('');
    const [onFeedBackViewClick, setOnFeedBackViewClick] = useState<(() => void) | undefined>(undefined);

    const params = useParams<{ id: string; }>()
    const router = useRouter();

    const showFeedback = (title: string, body:string, onClick?: Function) => {
        if (feedbackViewRef.current) {

            setfbhTitle(title);
            setfbhBody(body);
            setOnFeedBackViewClick(() => onClick); 

            feedbackViewRef.current.open();
        }
    };
    
    const OnComplete = async () => {
        
        setLoading(true);
        MarkModuleAsComplete(params.id)
        .catch(() => showFeedback("Error", "Something went wrong - Please try again later."))
        .finally(() => {router.back(); setLoading(false); });

    };

    let hasFetchedData = false;
    useEffect(() => {

        if(hasFetchedData == false){
            hasFetchedData = true;

            setLoading(true);

            GetCourseWords(params.id)
            .then((words: WordViewModel[]) => {

                if(words == null || words.length == 0)
                return;

                const unique = words.filter((val, id, array) => {
                return array.indexOf(val) == id;  
                });

                setWordViewModels(unique);
                setShowCourse(true);

            }).finally(() => setLoading(false));
        }
    }, []);

    return (
        <div>
            <Navbar /> 
            {showCourse && (<Multiselect words={wordViewModels} onDone={OnComplete}/>)}
            <FeedbackView ref={feedbackViewRef} title={fbTitle} body={fbBody} onClick={onFeedBackViewClick} />
            {loading && <SpinnerOverlay />}
        </div>
    );
}