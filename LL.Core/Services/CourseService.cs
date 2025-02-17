using System.Diagnostics;
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
    IDictionaryService dictionaryService) : ICourseService
{
    public async Task<CourseViewModel> CreateCourse(CourseRequestModel seminarRequest, int userId)
    {
        CourseViewModel courseViewModel = new CourseViewModel();

        if(seminarRequest == null || seminarRequest.IsValid == false) 
            return courseViewModel;
        
        courseViewModel.Id = courseRepository.Insert(seminarRequest.Text, seminarRequest.LanguageFromId, seminarRequest.LanguageToId, userId);
        
        string rankingPrompt = PromptFactory.CreateCourseWordsPrompt(seminarRequest.Text, seminarRequest.LanguageFromId);
        
        courseViewModel.Words = seminarRequest.Text.Trim().Contains(' ')
            ? JsonHelper.Extract<List<CourseWordsModel>>(await agentService.Run(rankingPrompt)) ?? []
            : [new CourseWordsModel(seminarRequest.Text, 1)];
        
        return courseViewModel;
    }
    public async Task<WordViewModel> CreateDefinitions(CourseRequestModel seminarRequest, int userId)
    {
        WordShort wordShort = wordRepository.Insert(seminarRequest.Text, seminarRequest.LanguageFromId, userId);
        
        WordLinkShort wordLink = wordLinkRepository.Insert(wordShort.Id, seminarRequest.Text, seminarRequest.LanguageFromId, seminarRequest.LanguageToId, userId);
            
        List<MeaningShort> wordMeaning = wordMeaningRepository.GetByWordId(wordShort.Id);
            
        if (wordMeaning == null)
        {
            string word = LanguageEnum.English.Equals(seminarRequest.LanguageFromId)
                ? seminarRequest.Text
                : wordLink.Target.Name;
                
            wordMeaning = word.Split(" ").SelectMany(w => dictionaryService.GetWordDetails(w).Result).ToList();
                
            foreach (var meaning in wordMeaning)
            {
                int wordMeaningId = wordMeaningRepository
                    .Insert(wordShort.Id, EnumHelper.GetEnumValue(typeof(WordTypeEnum), meaning.Type), userId);
            
                meaning.Definitions.ForEach(d => wordDefinitionRepository.Insert(d, wordMeaningId, userId));
            }
        }
            
        if(wordMeaning.Any())
            return new WordViewModel(wordShort, wordMeaning, wordLink.Target.Name);
        
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

