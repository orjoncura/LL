using LL.Core.Interfaces;
using LL.Extensions;
using LL.SharedDefinitions.Model;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LL.Core.Services
{
    public class ChatService : IChatService
    {
        public async Task<SeminarViewModel> CreateSeminar(SeminarRequestModel seminarRequest)
        {
            string contents = File.ReadAllText(@"D:\repos\LL\LL.Core\\Prompts\CreateSeminar.txt");

            string input = contents.Replace("{@words}", string.Join(',', seminarRequest.Words));

            SeminarViewModel? seminarViewModel = JSON.Extract<SeminarViewModel>(await Agent.Run(input));
            
            return seminarViewModel ?? new SeminarViewModel();
        }
    }
}
