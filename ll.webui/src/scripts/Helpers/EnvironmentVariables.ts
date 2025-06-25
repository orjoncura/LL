
import {EnvironmentVariables} from '@/scripts/models';

export function getEnv(): EnvironmentVariables {

  return {
    Application_Name: process.env.NEXT_PUBLIC_Application_Name || "",
    Version: process.env.NEXT_PUBLIC_Version || "",
    API_URL: process.env.NEXT_PUBLIC_API_URL || "",
    Token_Storage_Name: process.env.NEXT_PUBLIC_Token_Storage_Name || "",
    NODE_ENV: 'local'
  };
}
