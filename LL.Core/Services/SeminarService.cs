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
    ITranslationService translationService,
    IDictionaryService dictionaryService) : ISeminarService
{
    public async Task<List<SeminarViewModel>> CreateSeminar(SeminarRequestModel seminarRequest, int userId)
    {
        List<SeminarViewModel> seminarViewModels = new List<SeminarViewModel>();

        if(seminarRequest == null || seminarRequest.IsValid == false) 
            return seminarViewModels;
        
        int seminarId = seminarRepository.Insert(seminarRequest.Text, seminarRequest.LanguageFromId, seminarRequest.LanguageToId, userId);
        string rankingPrompt = PromptFactory.CreateSeminarWordsPrompt(seminarRequest.Text, seminarRequest.LanguageToId); 
        
        List<SeminarWordsModel> seminarWords = 
            JsonHelper.Extract<List<SeminarWordsModel>>(await agentService.Run(rankingPrompt)) 
            ?? new List<SeminarWordsModel>();
        
        foreach (var seminarWord in seminarWords)
        {
            int wordId = InsertWord(seminarWord.Word, seminarRequest.LanguageFromId, seminarRequest.LanguageToId, userId);
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

                seminarViewModels.Add(new SeminarViewModel(seminarWord.Word, statements, seminarWord.Importance));
            }
        }

        return seminarViewModels;
    }

    private int InsertWord(string word, int fromId, int toId, int userId)
    {
        int wordId = wordRepository.Insert(word, fromId, userId);

        string translatedWord = translationService.TranslateText(word, fromId, toId);
            
        int translatedWordId = wordRepository.Insert(translatedWord, fromId, userId);

        wordLinkRepository.Insert(wordId, translatedWordId, userId);

        InsertMeaningsByWordId(wordId, translatedWord, toId, userId);

        return wordId;
    }
    private void InsertMeaningsByWordId(int wordId, string word, int languageId, int userId)
    {
        if (wordMeaningRepository.Any(wordId) || !LanguageEnum.English.Equals(languageId))
            return;
        
        foreach (var meaning in dictionaryService.GetWordDetails(word).Result)
        {
            int wordMeaningId = wordMeaningRepository.Insert(wordId, meaning.TypeId, userId);
            
            meaning.Definitions.ForEach(d => wordDefinitionRepository.Insert(d, wordMeaningId, userId));
        }
    }
}

