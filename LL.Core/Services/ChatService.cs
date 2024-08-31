using LL.Core.Interfaces;
using LL.Data.Interfaces;
using LL.Extensions;
using LL.SharedDefinitions.Model;

namespace LL.Core.Services
{
    public class ChatService(IWordRepository wordRepository, IStatementRepository statementRepository) : IChatService
    {
        public async Task<List<SeminarViewModel>> CreateSeminar(SeminarRequestModel seminarRequest)
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
                        seminarViewModel.Sentence = statementRepository
                            .GetByWordId(targetWord.Id).Select(s => new StatementModel(s)).ToList();
                    }
                }
                else 
                {

                    string input = File.ReadAllText(@"D:\repos\LL\LL.Core\\Prompts\CreateSeminar.txt").Replace("{@targetWord}", word);
                    seminarViewModel = JSON.Extract<SeminarViewModel>(await Agent.Run(input));
                }

                if (seminarViewModel != null)
                    seminarViewModels.Add(seminarViewModel);
            }

            return seminarViewModels;
        }
    }
}
