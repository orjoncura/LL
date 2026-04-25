import {getEnv} from '@/utils/Models/EnvironmentVariables';
import {ApiResponse, ApiError} from '@/utils/Models/Types';
import {GetToken} from '@/utils/Security/AuthManager';

const api = getEnv().API_URL || '';
const offlineQueueKey = 'offline_request_queue_v1';
const getCachePrefix = 'offline_get_cache_v1:';

type OfflineQueuedRequest = {
    url: string;
    data: string;
    createdAt: number;
};

export async function POST<T>(url: string, data: string): Promise<any> {
    try {
        const response = await fetch(api + url, {
            method: 'POST',
            headers: GetHeader(),
            body: data,
        });

        return await HandleApiResponse<T>(response);
    }
    catch (error) {
        // Queue non-auth writes for later sync when app is offline.
        if (ShouldQueueRequest(url) && IsOfflineError(error as ApiError)) {
            QueueOfflineRequest(url, data);
        }

        return HandleApiError(error as ApiError);
    }
}

export async function GET<T>(url: string): Promise<any> {
    try {
        const response = await fetch(api + url, {
            method: 'GET',
            headers: GetHeader()
        });

        const output = await HandleApiResponse<T>(response);
        if (response.ok && output != null) {
            localStorage.setItem(GetCacheKey(url), JSON.stringify(output));
        }

        return output;
    }
    catch (error) {
        const cached = localStorage.getItem(GetCacheKey(url));
        if (cached) {
            try {
                return JSON.parse(cached) as T;
            } catch {
                localStorage.removeItem(GetCacheKey(url));
            }
        }

        return HandleApiError(error as ApiError);
    }
}

export async function CreateAudio(id: string | undefined) {
    return fetch(api + '/Course/StreamAudio?wordId=' + id, {
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

async function HandleApiResponse<T>(response: ApiResponse){

    try {
       if (response.status === 401) {
            location.replace("/")
       } else if (!response.ok){
            console.log(`HTTP error! status: ${response.status}`);
            return null;
       }else if(response.ok){
            return await response.json() as T;
       }
    } catch (e) {
       return null;
    }
}

function HandleApiError(error: ApiError){

    try {
        console.error('Error sending data:', error);

        // Keep users in-app while offline; callers can handle null responses gracefully.
        if (error instanceof Error && (error.message.includes('Connection refused')
            ||  error.message.includes('Failed to fetch'))) {
            return null;
        }
    
        return null;
    } catch (e) {
       return null;
    }
}

function ShouldQueueRequest(url: string): boolean {
    if (url.startsWith('/Security/'))
        return false;

    return true;
}

function IsOfflineError(error: ApiError): boolean {
    if (!(error instanceof Error))
        return false;

    return error.message.includes('Connection refused')
        || error.message.includes('Failed to fetch');
}

function QueueOfflineRequest(url: string, data: string): void {
    try {
        const queue = GetQueuedRequests();
        queue.push({ url, data, createdAt: Date.now() });
        localStorage.setItem(offlineQueueKey, JSON.stringify(queue));
    } catch {
        // Best-effort queueing only.
    }
}

function GetQueuedRequests(): OfflineQueuedRequest[] {
    try {
        const queueRaw = localStorage.getItem(offlineQueueKey);
        if (!queueRaw) return [];
        return JSON.parse(queueRaw) as OfflineQueuedRequest[];
    } catch {
        return [];
    }
}

function GetCacheKey(url: string): string {
    return `${getCachePrefix}${url}`;
}

export async function SyncOfflineQueue(): Promise<void> {
    const queue = GetQueuedRequests();
    if (queue.length === 0) return;

    const remaining: OfflineQueuedRequest[] = [];

    for (const request of queue) {
        try {
            const response = await fetch(api + request.url, {
                method: 'POST',
                headers: GetHeader(),
                body: request.data
            });

            if (!response.ok)
                remaining.push(request);
        } catch {
            remaining.push(request);
        }
    }

    localStorage.setItem(offlineQueueKey, JSON.stringify(remaining));
}