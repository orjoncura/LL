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
    IWordLinkRepository wordLinkRepository,
    IStatementRepository statementRepository,
    ISeminarWordRepository seminarWordRepository,
    IAgentService agentService,
    ITranslationService translationService) : ISeminarService
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
            var seminarViewModel = new SeminarViewModel();

            int wordId = wordRepository.Insert(seminarViewModel.TargetWord.Name,
                seminarViewModel.TargetWord.Definition,
                seminarViewModel.TargetWord.TypeId,
                seminarRequest.LanguageFromId,
                userId);

            int translatedWordId = wordRepository.Insert(
                translationService.TranslateText(seminarViewModel.TargetWord.Name,
                    seminarRequest.LanguageFromId, 
                    seminarRequest.LanguageToId), 
             translationService.TranslateText(seminarViewModel.TargetWord.Definition, 
                 seminarRequest.LanguageFromId, 
                 seminarRequest.LanguageToId),
                seminarViewModel.TargetWord.TypeId,
                seminarRequest.LanguageToId,
                userId);

            wordLinkRepository.Insert(wordId, translatedWordId, userId);
            
            int seminarWordId = seminarWordRepository.Insert(wordId, seminarId, seminarWord.Importance, userId);
            
            string prompt = PromptFactory.CreateSeminarPrompt(seminarWord.Word, seminarRequest.LanguageFromId, seminarRequest.LanguageToId);
            var statements = JsonHelper.Extract<List<StatementShort>>(await agentService.Run(prompt));

            if (statements != null && statements.Any(s => s.IsValid))
                statementRepository.InsertRange(seminarWordId, statements, userId);
        }

        return seminarViewModels;
    }
}

