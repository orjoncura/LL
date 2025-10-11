import {getEnv} from '@/utils/Models/EnvironmentVariables';
import {ApiResponse, ApiError} from '@/utils/Models/Types';
import {GetToken} from '@/utils/Security/AuthManager';

const api = getEnv().API_URL || '';

export async function POST(url: string, data: string): Promise<any> {
    return await fetch(api + url, {
        method: 'POST',
        headers: GetHeader(),
        body: data,
    })
    .then(response => HandleApiResponse(response))
    .catch(error => HandleApiError(error));
}

export async function GET(url: string): Promise<any> {
    return fetch(api + url, {
        method: 'GET',
        headers: GetHeader()
    })
    .then(response => HandleApiResponse(response))
    .catch(error => HandleApiError(error));
}

export async function CreateAudio(id: string | undefined) {
    return fetch(api + '/Speech/TextToSpeech?wordId=' + id, {
        method: 'POST',
        headers: GetHeader()
    })
    .then(response => {
        if (!response.ok) {
            // Handle non-2xx responses (e.g., 4xx, 5xx)
            console.log(`HTTP error! status: ${response.status}`);
        }else {
            console.log('Data sent successfully:');
        }

        return response.blob(); 
    }).then((blob) => {
        const blobUrl = URL.createObjectURL(blob);
        const audio = new Audio(blobUrl);
        audio.play().catch((err) => {
          console.error('Audio playback failed:', err);
        });

        // Optional: revoke the blob URL later to free memory
        audio.onended = () => {
          URL.revokeObjectURL(blobUrl);
        };
      })
    .catch(error => HandleApiError(error));
}

function GetHeader(){
    return {'Authorization': `Bearer ${GetToken()}`, 'Content-Type': 'application/json',}
}

function HandleApiResponse(response: ApiResponse){

    try {
       if (response.status === 401) {
            location.replace("/")
       } else if (!response.ok){
            console.log(`HTTP error! status: ${response.status}`);
            return null;
       }else if(response.ok){
            return response.json()
       }
    } catch (e) {
       return null;
    }
}

function HandleApiError(error: ApiError){

    try {
        console.error('Error sending data:', error);

        // Check if the error is related to a connection issue
        if (error instanceof Error && error.message.includes('Connection refused')
            ||  error.message.includes('Failed to fetch')) {

            // Redirect to login page
            location.replace("/")
        }
    
        return null;
    } catch (e) {
       return null;
    }
}