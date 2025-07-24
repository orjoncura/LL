using System.Text;
using System.Text.RegularExpressions;
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
        
        var splitTexts = Regex.Split(courseRequest.Text, @"(\r\n?|\n){2}")
            .Where(p => p.Any(char.IsLetterOrDigit) && !string.IsNullOrWhiteSpace(p))
            .ToList();
        
        int charLimit = 1241;
        List<string> combinedStrings = new List<string>();

        foreach (string text in splitTexts)
        {
            StringBuilder currentSegment = new StringBuilder();
    
            foreach (char c in text)
            {
                if (currentSegment.Length >= charLimit)
                    break;
        
                currentSegment.Append(c);
            }
    
            if (currentSegment.Length > 0)
            {
                if (combinedStrings.Count > 0 && 
                    combinedStrings[combinedStrings.Count - 1].Length + currentSegment.Length <= charLimit)
                {
                    combinedStrings[combinedStrings.Count - 1] += currentSegment.ToString();
                }
                else
                {
                    combinedStrings.Add(currentSegment.ToString());
                }
            }
        }
        
        List<CourseWordsModel> courseWordsList = new List<CourseWordsModel>();

        foreach (var text in combinedStrings)
        {
            var response = await agentService
                .Run(PromptFactory.CreateCourseWordsPrompt(text, courseRequest.LanguageFromId, courseRequest.LanguageToId));

            if (!string.IsNullOrEmpty(response))
            {
                // Safely extract the list of CourseWordsModel from JSON
                var extractedModels = JsonHelper.Extract<List<CourseWordsModel>>(response) ?? new List<CourseWordsModel>();
        
                courseWordsList.AddRange(extractedModels);
            }
        }
        
        courseWordsList = courseWordsList.Where(c => c.IsValid).ToList();
        
        words = courseWordsList.Select(cwl => new WordViewModel(cwl)).ToList();
        
        CreateDefinitions(courseId, courseWordsList, courseRequest.LanguageFromId, courseRequest.LanguageToId, userId);
        
        return words;
    }
    private void CreateDefinitions(int courseId, List<CourseWordsModel> courseWordsList, int fromId, int toId, int userId)
    {
        foreach (var courseWord in courseWordsList)
        {
            WordShort wordShort = wordRepository.Insert(courseWord.Word, fromId, userId);

            courseWordRepository.Insert(wordShort.Id, courseId, courseWord.Importance, userId);
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
    public async Task<List<ExerciseViewModel>> CreateExercises(int courseId, int userId)
    {
        List<ExerciseViewModel>? exercises = new List<ExerciseViewModel>();
        
        wordRepository.GetByCourseId(courseId);

        exercises = exerciseRepository.GetByCourseId(courseId);

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
            
                exerciseRepository.InsertRange(courseId, exercises, userId);
            }
        }
        
        return exercises ?? [];
    }
}

