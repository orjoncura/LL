export interface EnvironmentVariables {
  Application_Name: string;
  Version: string;
  API_URL: string;
  Token_Storage_Name: string;
}

export interface ApiResponse {
  status: number;
  ok: boolean;
  json: any;
}

export interface ApiError {
  message: string;
}