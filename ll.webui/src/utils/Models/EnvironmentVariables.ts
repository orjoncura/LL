
import {EnvironmentVariables} from '@/utils/Models/Types';

export function getEnv(): EnvironmentVariables {

  return {
    Application_Name: "Fluente",
    Version: process.env.NEXT_PUBLIC_Version || "",
    API_URL: process.env.NEXT_PUBLIC_API_URL || "",
    Token_Storage_Name: "JWT"
  };
}
