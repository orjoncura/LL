using LL.Core.Factories;
using LL.Core.Interfaces;
using LL.Data.Interfaces;
using LL.Extensions;
using LL.SharedDefinitions.Model;

namespace LL.Core.Services
{
    public class SeminarService(IWordRepository wordRepository, IStatementRepository statementRepository) : ISeminarService
    {
        public async Task<List<SeminarViewModel>> CreateSeminar(SeminarRequestModel seminarRequest, int userId)
        {
            List<SeminarViewModel> seminarViewModels = new List<SeminarViewModel>();

            foreach (var word in seminarRequest.Words) 
            {
                var seminarViewModel = new SeminarViewModel();

                if (wordRepository.Exist(word, seminarRequest.LanguageIdFrom, seminarRequest.LangaugeIdTo))
                {
                    var targetWord = wordRepository.GetSingleByName(word);

                    if (targetWord != null)
                    {
                        seminarViewModel.TargetWord = new StatementModel(targetWord.Name, targetWord.Translation);
                        seminarViewModel.Sentences = statementRepository
                            .GetByWordId(targetWord.Id).Select(s => new StatementModel(s)).ToList();
                    }
                }
                else 
                {
                    seminarViewModel = JSON.Extract<SeminarViewModel>(await Agent.Run(PromptFactory.CreateSeminarPrompt(word)));

                    if (seminarViewModel != null)
                    {
                        int wordId = wordRepository.Insert(seminarViewModel.TargetWord.OriginalStatement,
                            seminarViewModel.TargetWord.TranslatedStatement,
                            seminarRequest.LanguageIdFrom,
                            seminarRequest.LangaugeIdTo,
                            userId);

                        statementRepository.InsertRange(
                            seminarViewModel.ConvertToStatements(wordId,
                            seminarRequest.LanguageIdFrom,
                            seminarRequest.LangaugeIdTo,
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
