
import Constants from '../Constants'

export async function POST(url: string, data:string) {
    return fetch(Constants().API + url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: data,
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
