
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

export interface DeleteCourseWordModel {
  courseId?: number;
  wordId?: number;
}

export interface DefinitionRequestModel {
  text?: string;
  translation?: string;
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

export interface DateTimeViewModel {
  minute: number;
  day: number;
  month: number;
  asString:string
}

export interface CourseViewModel {
  id: number;
  text: string;
  createdDate: DateTimeViewModel;
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

