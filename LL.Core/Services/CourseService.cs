using System.Diagnostics;
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
    IAgentService agentService,
    IDictionaryService dictionaryService,
    ITranslationService translationService) : ICourseService
{
    public async Task<CourseViewModel> CreateCourse(CourseRequestModel seminarRequest, int userId)
    {
        CourseViewModel courseViewModel = new CourseViewModel();

        if(seminarRequest == null || seminarRequest.IsValid == false) 
            return courseViewModel;
        
        courseViewModel.Id = courseRepository.Insert(seminarRequest.Text, seminarRequest.LanguageFromId, seminarRequest.LanguageToId, userId);
        
        var splitTexts = Regex.Split(seminarRequest.Text, @"(\r\n?|\n){2}")
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
        
        Stopwatch stopwatch = new();
        stopwatch.Start();
        
        List<CourseWordsModel> courseWordsList = new List<CourseWordsModel>();

        foreach (var text in combinedStrings)
        {
            var prompt = PromptFactory.CreateCourseWordsPrompt(text, seminarRequest.LanguageFromId, seminarRequest.LanguageToId);
    
            try
            {
                var response = agentService.Run(prompt).Result;
        
                if (!string.IsNullOrEmpty(response))
                {
                    // Safely extract the list of CourseWordsModel from JSON
                    var extractedModels = JsonHelper.Extract<List<CourseWordsModel>>(response) ?? new List<CourseWordsModel>();
            
                    courseWordsList.AddRange(extractedModels);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing text: {text}\n{ex.Message}");
            }
        }

        courseViewModel.Words = courseWordsList;
        
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
        
        return courseViewModel;
    }
  
    public async Task<WordViewModel> CreateDefinitions(DefinitionRequestModel definitionRequestModel, int userId)
    {            
        List<string> translations = translationService
            .TranslateText(definitionRequestModel.Text,
                definitionRequestModel.LanguageFromId, 
                definitionRequestModel.LanguageToId).Result;

        definitionRequestModel.Translation = translations.FirstOrDefault(t => definitionRequestModel.Translation.ToLower().Contains(t.ToLower()));
        
        List<MeaningShort> wordMeaning = dictionaryService.GetWordDetails(definitionRequestModel.Translation).Result;
        
        if(string.IsNullOrEmpty(definitionRequestModel.Translation) || !wordMeaning.Any())
            return null;
        
        WordShort wordShort = wordRepository.Insert(definitionRequestModel.Text, definitionRequestModel.LanguageFromId, userId);
        
        WordLinkShort wordLink = wordLinkRepository.Insert(wordShort.Id, definitionRequestModel, userId);
        
        if (wordMeaningRepository.GetByWordId(wordShort.Id) == null)
        {
            foreach (var meaning in wordMeaning)
            {
                if (!Enum.TryParse(meaning.Type, true, out WordTypeEnum parsedValue))  // `true` for case-insensitive parsing
                    continue;
                
                int wordMeaningId = wordMeaningRepository
                    .Insert(wordShort.Id, (int)parsedValue, userId);
            
                meaning.Definitions.ForEach(d => wordDefinitionRepository.Insert(d, wordMeaningId, userId));
            }
        }
            
        if(wordMeaning.Any())
            return new WordViewModel(wordShort, wordMeaning, wordLink.Translation);
        
        return null;
    }
    public async Task<List<ExerciseViewModel>> CreateExercises(ExerciseRequestModel exerciseRequest, int userId)
    {
        List<ExerciseViewModel>? exercises = new List<ExerciseViewModel>();
        
        if (exerciseRequest.IsValid == false || userId == 0)
            return exercises; 
        
        int courseWordId = courseWordRepository.Insert(exerciseRequest.WordId, exerciseRequest.CourseId, exerciseRequest.RankId, userId);

        exercises = exerciseRepository.GetByCourseWordId(courseWordId);

        if (exercises.Any() == false)
        {
            string prompt = PromptFactory.CreateCoursePrompt(
                exerciseRequest.WordName,
                exerciseRequest.LanguageFromId, 
                exerciseRequest.LanguageToId, 
                exerciseRequest.Text);
        
            exercises = JsonHelper.Extract<List<ExerciseViewModel>>(await agentService.Run(prompt));
        
            if (exercises != null && exercises.Any(s => s.IsValid))
            {       
                exercises = exercises.Where(e => e.IsValid).ToList();
            
                exerciseRepository.InsertRange(courseWordId, exercises, userId);
            }
        }
        
        return exercises ?? [];
    }
}

