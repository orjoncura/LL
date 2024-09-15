using LL.Core.Enums;

namespace LL.Core.Factories
{
    public static class PromptFactory
    {
        public static string CreateSeminarPrompt(string word, int languageFromId, int languageToId, int numberOfSentences = 3)
        {
            string languageFrom = Enum.GetName(typeof(LanguageEnum), languageFromId) ?? string.Empty;
            string languageTo = Enum.GetName(typeof(LanguageEnum), languageToId) ?? string.Empty;

            string prompt = string.Format(@"Create {3} sentences in {2} for the word:{0} 
                and return a JSON file that contains the word, the sentences, 
                their translation to {1}, its definition and its type (verb and noun).
                If the word is in different language, explicit or isn't understandable, just ignore it.
                Please correct the grammar of the TargetWord if you have to and make sure all words start with a capital letter.
                Please make sure what you return is appropriate for kids.
                The JSON file should have the following format:

                {{
                    ""TargetWord"": {{
                        ""Name"": ""{0}"",
                        ""Translation"": """",
                        ""Definition"": """",
                        ""Type"": """",
                    }},
                    ""Sentences"": [
                        {{
                            ""OriginalStatement"": """",
                            ""TranslatedStatement"": """",
                        }},
                        {{
                            ""OriginalStatement"": """",
                            ""TranslatedStatement"": """",
                        }},
                        {{
                            ""OriginalStatement"": """",
                            ""TranslatedStatement"": """",
                        }}
                    ]
                }}", word, languageFrom, languageTo, numberOfSentences);

            return prompt;
        }

        public static string CreateRankingPrompt(string text, int languageFromId, int languageToId)
        {
            string languageFrom = Enum.GetName(typeof(LanguageEnum), languageFromId) ?? string.Empty;
            string languageTo = Enum.GetName(typeof(LanguageEnum), languageToId) ?? string.Empty;

            string prompt = string.Format(@"Rank the following word by their importance in the sentence. 
                By importance i mean, how important they are to understand the given sentence", text, languageFrom, languageTo);

            return prompt;
        }
    }
}
