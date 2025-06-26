import {getEnv} from '@/scripts/Helpers/EnvironmentVariables';

const env = getEnv();
const tokenStorageName = env.Token_Storage_Name || '';
const api = env.API_URL || '';

export async function POST(url: string, data: string): Promise<any> {
    return await fetch(api + url, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${GetToken()}`,
            'Content-Type': 'application/json',
        },
        body: data,
    })
    .then(response => {

        if (!response.ok) {
            console.log(`HTTP error! status: ${response.status}`);
            return null;
        }else {

            console.log('Data sent successfully:');

            try {
                return response.json(); 
            } catch (e) {
                return null;
            }
        }
    })
    .catch(error => {
        console.error('Error sending data:', error);

        // Check if the error is related to a connection issue
        if (error instanceof Error && error.message.includes('Connection refused')
            ||  error.message.includes('Failed to fetch')) {

            // Redirect to login page
            location.replace("/")
        }
    
        return null;
    });
}

export async function GET(url: string): Promise<any> {
    return fetch(api + url, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${GetToken()}`,
            'Content-Type': 'application/json',
        }
    })
    .then(response => {
        if (!response.ok) {
            // Handle non-2xx responses (e.g., 4xx, 5xx)
            console.log(`HTTP error! status: ${response.status}`);
        }else {
            console.log('Data sent successfully:');
        }
        return response.json();  // Parse the JSON response
    })
    .catch(error => {
        console.error('Error sending data:', error);

        // Check if the error is related to a connection issue
        if (error instanceof Error && error.message.includes('Connection refused')
            ||  error.message.includes('Failed to fetch')) {

            // Redirect to login page
            location.replace("/")
        }

        throw error;  // Re-throw the error so it can be caught by the caller
    });
}

export async function CreateAudio(id: number | undefined) {
    return fetch(api + '/Course/StreamAudio?wordId=' + id, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${GetToken()}`,
            'Content-Type': 'application/json',
        }
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
    .catch(error => {
        console.error('Error sending data:', error);
        throw error;  // Re-throw the error so it can be caught by the caller
    });
}

export function StoreToken(token: string) {

    localStorage.setItem(tokenStorageName, token);
}

export function GetToken(): string | null {
    return localStorage.getItem(tokenStorageName);
}

export function RemoveToken() {
    localStorage.removeItem(tokenStorageName);
}

