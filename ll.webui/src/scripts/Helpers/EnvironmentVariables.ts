
import {EnvironmentVariables} from '@/scripts/models';

export function getEnv(): EnvironmentVariables {
  const { NEXT_PUBLIC_Application_Name, Version, API_URL, Token_Storage_Name, NODE_ENV } = process.env;

  if (!NEXT_PUBLIC_Application_Name) throw new Error('Missing Application_Name');
  if (!Version) throw new Error('Missing Version');
  if (!API_URL) throw new Error('Missing API_URL');
  if (!Token_Storage_Name) throw new Error('Missing Token_Storage_Name');
  if (!NODE_ENV) throw new Error('Missing NODE_ENV');

  return {
    Application_Name: NEXT_PUBLIC_Application_Name,
    Version,
    API_URL,
    Token_Storage_Name,
    NODE_ENV: NODE_ENV as 'local' | 'development' | 'production' 
  };
}
