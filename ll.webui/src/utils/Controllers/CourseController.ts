import { GET, POST, CreateAudio } from '@/utils/Security/httpClient'

import 
{
   WordViewModel, 
   CourseRequestModel, 
   CourseViewModel, 
   ExerciseViewModel,
   ModuleViewModel, 
   DeleteCourseWordModel,
   SetWordDifficultyModel, 
   GetWordsByDifficultyModel 
} from '@/utils/Models/models';

export async function GetKeyWords(languageId: number): Promise<WordViewModel[]> {
  const response = await GET<WordViewModel[]>('/Course/GetKeyWords?languageId=' +  languageId);

  if (!response || response.length === 0) {
    return [];
  }

  return response;
}

export async function Create(courseRequestModel: CourseRequestModel): Promise<boolean> {
    return await POST<boolean>('/Course/Create', JSON.stringify(courseRequestModel));
}

export async function GetCourses(): Promise<CourseViewModel[]> {
  const response = await GET<CourseViewModel[]>('/Course/GetCourses');

  if (!response || response.length === 0) {
    return [];
  }

  return response;
}

export async function DeleteCourseById(courseId: string): Promise<boolean> {
  return await GET<boolean>('/Course/DeleteCourseById?courseId=' +  courseId);
}

export async function GetExercisesByModuleId(moduleId: string): Promise<ExerciseViewModel[]> {
  const response = await GET<ExerciseViewModel[]>('/Course/GetExercisesByModuleId?moduleId=' + moduleId);

  if (!response || response.length === 0) {
    return [];
  }

  return response;
}

export async function MarkModuleAsComplete(moduleId: string): Promise<boolean> {
  return await GET<boolean>('/Course/MarkModuleAsComplete?moduleId=' + moduleId);
}

export async function GetCourseWords(moduleId: string): Promise<WordViewModel[]> {
  const response = await GET<WordViewModel[]>('/Course/GetCourseWords?moduleId=' + moduleId);

  if (!response || response.length === 0) {
    return [];
  }

  return response;
}

export async function GetModulesByCourseId(moduleId: string): Promise<ModuleViewModel[]> {
  const response = await GET<ModuleViewModel[]>('/Course/GetModulesByCourseId?courseId=' + moduleId);

  if (!response || response.length === 0) {
    return [];
  }

  return response;
}
export async function DeleteCourseWord(moduleId: string, wordId: string): Promise<boolean> {

    const data: DeleteCourseWordModel = { "moduleId": moduleId, "wordId": wordId };

    return await POST<boolean>('/Course/DeleteCourseWord', JSON.stringify(data));
}

export async function GetWordsByDifficulty(languageId: number, difficultyId: number): Promise<WordViewModel[]> {

    const data: GetWordsByDifficultyModel = { "languageId": languageId, "difficultyId": difficultyId};

    return await POST<WordViewModel[]>('/Course/GetWordsByDifficulty', JSON.stringify(data));
}

export async function GetWordCountByDifficulty(languageId: number, difficultyId: number): Promise<number> {

    const data: GetWordsByDifficultyModel = { "languageId": languageId, "difficultyId": difficultyId};

    return await POST<number>('/Course/GetWordCountByDifficulty', JSON.stringify(data));
}

export async function SetWordDifficulty(difficultyId: number, wordId: string): Promise<boolean> {

    const data: SetWordDifficultyModel = {"difficultyId": difficultyId, "wordId": wordId};

    return await POST<boolean>('/Course/SetWordDifficulty', JSON.stringify(data));
}

export async function StreamAudio(wordId: string): Promise<void> {

    await CreateAudio(wordId);
}