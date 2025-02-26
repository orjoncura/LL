import * as coreClient from "@azure/core-client";

export interface ConfirmationModel {
  password: string;
  confirmPassword: string;
  token: string;
  email: string;
}

export interface NewUserModel {
  email: string;
  confirmEmail: string;
}

export interface NewPasswordModel {
  password: string;
  confirmPassword: string;
}

export interface LoginModel {
  email: string;
  password: string;
  /** NOTE: This property will not be serialized. It can only be populated by the server. */
  readonly isValid?: boolean;
}

export interface TokenViewModel {
  token?: string;
  expiration?: string;
}

export interface ProblemDetails {
  /** Describes unknown properties. The value of an unknown property can be of "any" type. */
  [property: string]: any;
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  instance?: string;
}

export interface CourseRequestModel {
  text?: string;
  languageFromId?: number;
  languageToId?: number;
  /** NOTE: This property will not be serialized. It can only be populated by the server. */
  readonly isValid?: boolean;
}

export interface ExerciseRequestModel {
  courseId: number;
  text?: string;
  languageFromId?: number;
  languageToId?: number;  
  
  wordId?: number;
  wordName?: string;
  rankId?: number;
}

export interface CourseViewModel {
  id: number;
  words: CourseWordsModel[];
  /** NOTE: This property will not be serialized. It can only be populated by the server. */
  readonly isValid?: boolean;
}

export interface CourseWordsModel {
  word: string;
  importance: number;
}

export interface WordViewModel {
  id: number;
  name: string;
  translation?: string;
  audio: Uint8Array;
  language?: string;
  meanings?: meaningShort[];
  importanceRatingId: number
}

export interface meaningShort {
  type?: number;
  definitions?: string[];
}

export interface ExerciseViewModel {
  original?: string;
  translated?: string;
  extra?: string;
  /** NOTE: This property will not be serialized. It can only be populated by the server. */
  readonly isValid?: boolean;
}

/** Optional parameters. */
export interface ApiSecurityOptionalParams extends coreClient.OperationOptions {
  body?: LoginModel;
}

/** Contains response data for the security operation. */
export type ApiSecurityResponse = TokenViewModel[];

/** Optional parameters. */
export interface ApiSeminarOptionalParams extends coreClient.OperationOptions {
  body?: CourseRequestModel;
}

/** Contains response data for the seminar operation. */
export type ApiSeminarResponse = CourseViewModel[];

/** Optional parameters. */
export interface ApiOptionalParams extends coreClient.ServiceClientOptions {
  /** Overrides client endpoint. */
  endpoint?: string;
}
