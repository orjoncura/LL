
import Constants from '../Constants'

export async function POST(url: string, data: string): Promise<any> {
    return await fetch(Constants().API + url, {
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
        return null;
    });
}

export async function GET(url: string) {
    return fetch(Constants().API + url, {
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
        throw error;  // Re-throw the error so it can be caught by the caller
    });
}


export async function CreateAudio(id: number | undefined) {
    return fetch(Constants().API + '/Course/StreamAudio?wordId=' + id, {
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

        return response.arrayBuffer();  // Parse the JSON response
    }).then(arrayBuffer => {
        const audioContext = new AudioContext();
        audioContext.decodeAudioData(arrayBuffer, (buffer) => {
            console.log('Audio decoded successfully');
            const source = audioContext.createBufferSource();
            source.buffer = buffer;
            source.connect(audioContext.destination);
            source.start(0);
        }, (error) => {
            console.error('Error decoding audio data:', error);
        });
    })
    .catch(error => {
        console.error('Error sending data:', error);
        throw error;  // Re-throw the error so it can be caught by the caller
    });
}

export function StoreToken(token: string) {

    localStorage.setItem(Constants().TokenStorageName, token);
}

export function GetToken(): string | null {
    return localStorage.getItem(Constants().TokenStorageName);
}

export function RemoveToken() {
    localStorage.removeItem(Constants().TokenStorageName);
}

