using LL.Core.Enums;
using LL.Core.Factories;
using LL.Core.Helpers;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;

namespace LL.Core.Services;

public class CourseService(
    ICourseRepository courseRepository,
    IModuleRepository moduleRepository,
    IWordRepository wordRepository,
    IWordMeaningRepository wordMeaningRepository,
    IWordDefinitionRepository wordDefinitionRepository,
    IWordLinkRepository wordLinkRepository,
    IExerciseRepository exerciseRepository,
    ICourseWordRepository courseWordRepository,
    IAgentService agentService) : ICourseService
{
    public async Task<List<WordViewModel>> CreateCourse(CourseRequestModel courseRequest, int userId)
    {
        List<WordViewModel> words = new List<WordViewModel>();
        
        if(courseRequest.IsValid == false) 
            return words;
        
        int courseId = courseRepository.Insert(courseRequest.Text, courseRequest.LanguageFromId, courseRequest.LanguageToId, userId);
        
        int index = 0;
        foreach (var text in SplitText(courseRequest.Text))
        {
            var response = await agentService
                .Run(PromptFactory.CreateCourseWordsPrompt(text, courseRequest.LanguageFromId, courseRequest.LanguageToId));

            if (!string.IsNullOrEmpty(response))
            {
                // Safely extract the list of CourseWordsModel from JSON
                var extractedModels = JsonHelper.Extract<List<CourseWordsModel>>(response) ?? new List<CourseWordsModel>();
                words.AddRange(extractedModels.Select(cwl => new WordViewModel(cwl)).ToList());

                string moduleTitleSufix = ": Part " + index + 1;
                int moduleId = moduleRepository.Insert(courseId, "Flashcards" + moduleTitleSufix, (int)ModuleTypeEnum.Flashcards, true, userId);
                CreateDefinitions(moduleId, extractedModels, courseRequest.LanguageFromId, courseRequest.LanguageToId, userId);
                moduleRepository.Insert(courseId, "Exercises" + moduleTitleSufix, (int)ModuleTypeEnum.Exercises, false, userId);
            }

            index++;
        }
        
        return words;
    }
    public async Task<List<ExerciseViewModel>> CreateExercises(string moduleId, int userId)
    {
        List<ExerciseViewModel>? exercises = new List<ExerciseViewModel>();
        
        wordRepository.GetByModuleId(moduleId);

        exercises = exerciseRepository.GetByModuleId(moduleId);

        if (exercises.Any() == false)
        {
            /*string prompt = PromptFactory.CreateCoursePrompt(
                exerciseRequest.WordName,
                exerciseRequest.LanguageFromId,
                exerciseRequest.LanguageToId,
                exerciseRequest.Text);

            exercises = JsonHelper.Extract<List<ExerciseViewModel>>(await agentService.Run(prompt));*/
        
            if (exercises != null && exercises.Any(s => s.IsValid))
            {       
                exercises = exercises.Where(e => e.IsValid).ToList();
            
                exerciseRepository.InsertRange(moduleId, exercises, userId);
            }
        }
        
        return exercises ?? [];
    }
    
    private List<string> SplitText(string input, int minWords = 100, int maxWords = 350)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new List<string>();

        string[] words = input.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        int totalWords = words.Length;

        // Calculate the number of chunks needed
        int chunkCount = (int)Math.Ceiling((double)totalWords / maxWords);
        int baseChunkSize = totalWords / chunkCount;
        int remainder = totalWords % chunkCount;

        List<string> result = new List<string>();
        int index = 0;

        for (int i = 0; i < chunkCount; i++)
        {
            int currentChunkSize = baseChunkSize + (i < remainder ? 1 : 0);

            // Ensure the chunk size respects the min/max bounds
            if (currentChunkSize < minWords && i > 0)
            {
                // Merge with the previous chunk if too small
                string last = result[result.Count - 1];
                result.RemoveAt(result.Count - 1);
                currentChunkSize += last.Split(' ').Length;
                string merged = string.Join(" ", last, string.Join(" ", words.Skip(index).Take(currentChunkSize)));
                result.Add(merged);
            }
            else
            {
                string chunk = string.Join(" ", words.Skip(index).Take(currentChunkSize));
                result.Add(chunk);
            }

            index += currentChunkSize;
        }

        return result;
    }
    private void CreateDefinitions(int moduleId, List<CourseWordsModel> courseWordsList, int fromId, int toId, int userId)
    {
        foreach (var courseWord in courseWordsList)
        {
            WordShort wordShort = wordRepository.Insert(courseWord.Word, fromId, userId);

            courseWordRepository.Insert(wordShort.Id, moduleId, courseWord.Importance, userId);
            wordLinkRepository.Insert(wordShort.Id, courseWord.Translation, fromId, toId, userId);

            MeaningShort meaning = new MeaningShort
            {
                Definitions = new List<string>()
                {
                    courseWord.Definition
                },
                Type = courseWord.PartOfSpeech
            };

            if (wordMeaningRepository.GetByWordId(wordShort.Id) == null)
            {
                if (!Enum.TryParse(meaning.Type, true,
                        out WordTypeEnum parsedValue)) // `true` for case-insensitive parsing
                    continue;

                int wordMeaningId = wordMeaningRepository
                    .Insert(wordShort.Id, (int)parsedValue, userId);

                meaning.Definitions.ForEach(d => wordDefinitionRepository.Insert(d, wordMeaningId, userId));
            }
        }
    }
}

