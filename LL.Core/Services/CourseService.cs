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
    IStatementRepository statementRepository,
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
        string rankingPrompt = PromptFactory.CreateCourseWordsPrompt(seminarRequest.Text, seminarRequest.LanguageToId); 
        
        List<CourseWordsModel> seminarWords = 
            JsonHelper.Extract<List<CourseWordsModel>>(await agentService.Run(rankingPrompt)) 
            ?? new List<CourseWordsModel>();
        
        foreach (var seminarWord in seminarWords)
        {
            WordShort wordShort = wordRepository.Insert(seminarWord.Word, seminarRequest.LanguageFromId, userId);
        
            WordLinkShort wordLink = wordLinkRepository.Insert(wordShort.Id, seminarWord.Word, seminarRequest.LanguageFromId, seminarRequest.LanguageToId, userId);
            
            List<MeaningShort> wordMeaning = wordMeaningRepository.GetByWordId(wordShort.Id);
            
            if (wordMeaning == null)
            {
                string word = LanguageEnum.English.Equals(seminarRequest.LanguageFromId)
                    ? seminarWord.Word
                    : wordLink.Target.Name;
                
                wordMeaning = word.Split(" ").SelectMany(w => dictionaryService.GetWordDetails(w).Result).ToList();
                
                foreach (var meaning in wordMeaning)
                {
                    int wordMeaningId = wordMeaningRepository
                        .Insert(wordShort.Id, EnumHelper.GetEnumValue(typeof(WordTypeEnum), meaning.Type), userId);
            
                    meaning.Definitions.ForEach(d => wordDefinitionRepository.Insert(d, wordMeaningId, userId));
                }
            }
            
            courseViewModel.Words.Add(new WordViewModel(wordShort, wordMeaning, seminarWord.Importance, wordLink.Target.Name));
        }

        return courseViewModel;
    }
    
    public async Task<List<ExerciseViewModel>> CreateExercises(ExerciseRequestModel exerciseRequest, int userId)
    {
        List<ExerciseViewModel> exerciseViewModel = new List<ExerciseViewModel>();
        
        if (exerciseRequest == null || exerciseRequest.IsValid == false || userId == 0)
            return exerciseViewModel; 
        
        int courseWordId = courseWordRepository.Insert(exerciseRequest.WordId, exerciseRequest.CourseId, exerciseRequest.RankId, userId);
        
        string prompt = PromptFactory.CreateCoursePrompt(
            exerciseRequest.WordName,
            exerciseRequest.LanguageFromId, 
            exerciseRequest.LanguageToId, 
            exerciseRequest.Text);
        
        var exercises = JsonHelper.Extract<List<ExerciseShort>>(await agentService.Run(prompt));
        
        if (exercises != null && exercises.Any(s => s.IsValid))
        {
            statementRepository.InsertRange(courseWordId, exercises, userId);
        
            exerciseViewModel = exercises
                .Where(e => e.IsValid)
                .Select(e => new ExerciseViewModel(e)).ToList();
        }
        
        return exerciseViewModel;
    }
}

