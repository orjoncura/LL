using LL.Core.Enums;
using LL.Core.Factories;
using LL.Core.Interfaces;
using LL.Data.Interfaces;
using LL.Extensions;
using LL.SharedDefinitions.Models;

namespace LL.Core.Services
{
    public class SeminarService(IWordRepository wordRepository, IStatementRepository statementRepository) : ISeminarService
    {
        public async Task<List<SeminarViewModel>> CreateSeminar(SeminarRequestModel seminarRequest, int userId)
        {
            List<SeminarViewModel> seminarViewModels = new List<SeminarViewModel>();

            if(seminarRequest == null || seminarRequest.IsValid == false) 
                return seminarViewModels;

            seminarRequest.Words = seminarRequest.Words.Select(w => w.ToLower()).Distinct().ToList();

            foreach (var word in seminarRequest.Words) 
            {
                var seminarViewModel = new SeminarViewModel();

                if (wordRepository.Exist(word, seminarRequest.LanguageFromId, seminarRequest.LangaugeToId))
                {
                    var targetWord = wordRepository.GetSingleByName(word);

                    if (targetWord != null)
                    {
                        seminarViewModel.TargetWord = new TargetWordViewModel(targetWord);
                        seminarViewModel.Sentences = statementRepository
                            .GetByWordId(targetWord.Id).Select(s => new StatementModel(s)).ToList();
                    }
                }
                else if(seminarViewModel != null)
                {
                    string prompt = PromptFactory.CreateSeminarPrompt(word, seminarRequest.LanguageFromId, seminarRequest.LangaugeToId);
                    seminarViewModel = JSON.Extract<SeminarViewModel>(await Agent.Run(prompt));

                    if (seminarViewModel != null && seminarViewModel.IsValid)
                    {
                        int wordId = wordRepository.Insert(seminarViewModel.TargetWord.Name,
                            seminarViewModel.TargetWord.Translation,
                            seminarViewModel.TargetWord.Definition,
                            seminarViewModel.TargetWord.Type,
                            seminarRequest.LanguageFromId,
                            seminarRequest.LangaugeToId,
                            userId);

                        statementRepository.InsertRange(
                            seminarViewModel.ConvertToStatements(wordId,
                            seminarRequest.LanguageFromId,
                            seminarRequest.LangaugeToId,
                            userId));
                    }
                }

                if (seminarViewModel != null)
                    seminarViewModels.Add(seminarViewModel);
            }

            return seminarViewModels;
        }
    }
}
