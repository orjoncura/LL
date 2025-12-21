
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
  title?: string;
  url?: string;
  text?: string;
  languageFromId?: number;
  languageToId?: number;
  /** NOTE: This property will not be serialized. It can only be populated by the server. */
  readonly isValid?: boolean;
}

export interface DeleteCourseWordModel {
  moduleId?: string;
  wordId?: string;
}

export interface DefinitionRequestModel {
  text?: string;
  translation?: string;
  languageFromId?: number;
  languageToId?: number;
  /** NOTE: This property will not be serialized. It can only be populated by the server. */
  readonly isValid?: boolean;
}

export interface ModuleViewModel {
  id: string;
  title?: string;
  typeId?: number;
  unlocked?: boolean;  
  completed?: boolean;
}

export interface DateTimeViewModel {
  minute: number;
  day: number;
  month: number;
  asString:string
}

export interface CourseViewModel {
  id: string;
  text: string;
  createdDate: DateTimeViewModel;
}

export interface WordViewModel {
  id: string;
  name: string;
  translation?: string;
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

export interface ProfileViewModel {
  email: string;
  dateCreated: DateTimeViewModel;
}

export interface VideoInfo {
    title: string;
    author_name?: string; // Optional field
    author_url?: string; // Optional field
    type: string;
    height: number;
    width: number;
    version: string;
    provider_name: string;
    provider_url: string;
    thumbnail_height: number;
    thumbnail_width: number;
    thumbnail_url: string;
    html: string;
}