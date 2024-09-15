using LL.Core.Factories;
using LL.Core.Interfaces;
using LL.Core.Models;
using LL.Data.Interfaces;
using LL.Extensions;
using LL.Extensions.Models;
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
            
            string rankingPrompt = PromptFactory.CreateSeminarWordsPrompt(seminarRequest.Text, seminarRequest.LanguageToId); 
            var seminarWords = JSON.Extract<List<SeminarWordsModel>>(await Agent.Run(rankingPrompt));
            
            foreach (var seminarWord in seminarWords) 
            {
                var seminarViewModel = new SeminarViewModel();

                if (wordRepository.Exist(seminarWord.Word, seminarRequest.LanguageFromId, seminarRequest.LanguageToId))
                {
                    var targetWord = wordRepository.GetSingleByName(seminarWord.Word);

                    if (targetWord != null)
                    {
                        seminarViewModel.TargetWord = new TargetWordViewModel(targetWord);
                        seminarViewModel.Sentences = statementRepository
                            .GetByWordId(targetWord.Id).Select(s => new StatementModel(s)).ToList();
                    }
                }
                else if(seminarViewModel != null)
                {
                    string prompt = PromptFactory.CreateSeminarPrompt(seminarWord.Word, seminarRequest.LanguageFromId, seminarRequest.LanguageToId);
                    seminarViewModel = JSON.Extract<SeminarViewModel>(await Agent.Run(prompt));

                    if (seminarViewModel != null && seminarViewModel.IsValid)
                    {
                        int wordId = wordRepository.Insert(seminarViewModel.TargetWord.Name,
                            seminarViewModel.TargetWord.Translation,
                            seminarViewModel.TargetWord.Definition,
                            seminarViewModel.TargetWord.Type,
                            seminarRequest.LanguageFromId,
                            seminarRequest.LanguageToId,
                            userId);

                        statementRepository.InsertRange(
                            seminarViewModel.ConvertToStatements(wordId,
                            seminarRequest.LanguageFromId,
                            seminarRequest.LanguageToId,
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
