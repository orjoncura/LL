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

public class SeminarService(
    ISeminarRepository seminarRepository,
    IWordRepository wordRepository,
    IWordMeaningRepository wordMeaningRepository,
    IWordDefinitionRepository wordDefinitionRepository,
    IWordLinkRepository wordLinkRepository,
    IStatementRepository statementRepository,
    ISeminarWordRepository seminarWordRepository,
    IAgentService agentService,
    IDictionaryService dictionaryService) : ISeminarService
{
    public async Task<SeminarViewModel> CreateSeminar(SeminarRequestModel seminarRequest, int userId)
    {
        SeminarViewModel seminarViewModel = new SeminarViewModel();

        if(seminarRequest == null || seminarRequest.IsValid == false) 
            return seminarViewModel;
        
        seminarViewModel.SeminarId = seminarRepository.Insert(seminarRequest.Text, seminarRequest.LanguageFromId, seminarRequest.LanguageToId, userId);
        string rankingPrompt = PromptFactory.CreateSeminarWordsPrompt(seminarRequest.Text, seminarRequest.LanguageToId); 
        
        List<SeminarWordsModel> seminarWords = 
            JsonHelper.Extract<List<SeminarWordsModel>>(await agentService.Run(rankingPrompt)) 
            ?? new List<SeminarWordsModel>();
        
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
            
            seminarViewModel.Words.Add(new WordViewModel(wordShort, wordMeaning, seminarWord.Importance, wordLink.Target.Name));
        }

        return seminarViewModel;
    }
    
    public async Task<List<StatementViewModel>> CreateSentences(SeminarWordsModel seminarWord, SeminarRequestModel seminarRequest, int wordId, int seminarId, int userId)
    {
        List<StatementViewModel> statementViewModels = new List<StatementViewModel>();
        
        int seminarWordId = seminarWordRepository.Insert(wordId, seminarId, seminarWord.Importance, userId);
        
        string prompt = PromptFactory.CreateSeminarPrompt(
            seminarWord.Word,
            seminarRequest.LanguageFromId, 
            seminarRequest.LanguageToId,
            seminarRequest.Text);
        
        var statements = JsonHelper.Extract<List<StatementShort>>(await agentService.Run(prompt));
        
        if (statements != null && statements.Any(s => s.IsValid))
        {
            statementRepository.InsertRange(seminarWordId, statements, userId);
        
            statementViewModels.Add(new StatementViewModel(seminarWord.Word, statements, seminarWord.Importance));
        }
        
        return statementViewModels;
    }
}

