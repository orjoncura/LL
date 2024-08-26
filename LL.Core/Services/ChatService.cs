using LL.Core.Interfaces;
using LL.Extensions;
using LL.SharedDefinitions.Model;

namespace LL.Core.Services
{
    public class ChatService : IChatService
    {
        public async Task<SeminarViewModel> CreateSeminar(SeminarRequestModel seminarRequest)
        {
            string input = "Create 3 sentenses in Spanish for each of following words " + seminarRequest.Words.ToString() 
                + " and return a json file that contains the words, the senteces and their translation to English"
                + "the json file should has the following format {\r\n    \"TargetWord\": {\r\n    " +
                "    \"OriginalStatement\": \"example\",\r\n        \"TranslatedStatement\": \"ejemplo\",\r\n    " +
                "    \"LanguageFrom\": 1,\r\n        \"LanguageTo\": 2\r\n    },\r\n    \"Sentence\": [\r\n      " +
                "  {\r\n            \"OriginalStatement\": \"This is an example sentence.\",\r\n          " +
                "  \"TranslatedStatement\": \"Esta es una oración de ejemplo.\",\r\n         " +
                "   \"LanguageFrom\": 1,\r\n            \"LanguageTo\": 2\r\n        },\r\n        {\r\n          " +
                "  \"OriginalStatement\": \"Another example sentence.\",\r\n           " +
                " \"TranslatedStatement\": \"Otra oración de ejemplo.\",\r\n    " +
                "        \"LanguageFrom\": 1,\r\n            \"LanguageTo\": 2\r\n        }\r\n    ]\r\n}\r\n";

            string t = await Agent.Run(input, "\"D:\\GGUF\\7B-chat.gguf\"");
            
            return new SeminarViewModel();
        }
    }
}
