import {getEnv} from '@/utils/Models/EnvironmentVariables';


const tokenStorageName = getEnv().Token_Storage_Name || '';


export function StoreToken(token: string) {

    localStorage.setItem(tokenStorageName, token);
}

export function GetToken(): string | null {
    return localStorage.getItem(tokenStorageName);
}

export function RemoveToken() {
    localStorage.removeItem(tokenStorageName);
}

