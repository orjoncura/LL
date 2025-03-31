using LL.Core.Enums;

namespace LL.Core.Factories
{
    public static class PromptFactory
    {
        public static string CreateCoursePrompt(string word, int languageFromId, int languageToId, string text, int numberOfSentences = 3)
        {
            string languageFrom = Enum.GetName(typeof(LanguageEnum), languageFromId) ?? string.Empty;
            string languageTo = Enum.GetName(typeof(LanguageEnum), languageToId) ?? string.Empty;

            string prompt = string.Format(@"Create {4} sentences in {1} for the word:{0} 
                and return a JSON file that contains the new statement and its translation to {2}
                If the word is in different language, explicit or isn't understandable, just ignore it.
                Please make sure what you return is appropriate for kids.
                Please make sure {0} in the new sentences has the same meaning, it has in the text,
                also try to use only words that are included in the text.
                The text is {3}
                Also add extra words in the same language as the translated statement, 
                if the translated statement is 'I want to drive' the extra words can be 'You can train'.
                The extra words will be used in a multi select exercise to confuse the user

                The JSON file should have the following format:
                [
                  {{
                    ""Original"": """",
                    ""Translated"": """",
                    ""Extra"": """"
                  }},
                  {{
                    ""Original"": """",
                    ""Translated"": """",
                    ""Extra"": """"
                  }},
                  {{
                    ""Original"": """",
                    ""Translated"": """",
                    ""Extra"": """"
                  }}
                ]", word, languageFrom, languageTo, text, numberOfSentences);

            return prompt;
        }

        public static string CreateCourseWordsPrompt(string text, int languageFromId, int languageToId)
        {
            string languageFrom = Enum.GetName(typeof(LanguageEnum), languageFromId) ?? string.Empty;
            string languageTo = Enum.GetName(typeof(LanguageEnum), languageToId) ?? string.Empty;

            string prompt = string.Format
            (@"Rank the following sentence's words by their importance to understand it in context. 
               By 'importance,' I mean how essential each word is for conveying the sentence's meaning. Only include words in {1} if they are understandable and relevant.
               Ignore any words that are unclear, explicit, or not in the given context. Do not include them in the JSON output.

                For ranking:

                Words critical to the main meaning of the sentence (e.g., nouns, verbs, key concepts) are ranked as '1'.
                Less important words (e.g., adverbs, prepositions) are ranked as '2'.
                Function words (e.g., articles, conjunctions) are ranked as '3'.
                Additionally:

                Return only a JSON file with the ranking and translations.
                If a word is part of another language (e.g., 'su vida'), split it into individual words ('su' and 'vida') unless separating them changes meaning (e.g., 'otra vez' remains together).
                Provide translations to {2} for each ranked word in the context they appear.
                Example:

                The JSON file should have the following format:

                [
                  {{
                    ""Word"": ""sentence"",
                    ""Translation"": ""oración"",
                    ""Importance"": 1
                  }},
                  {{
                    ""Word"": ""typically"",
                    ""Translation"": ""típicamente"",
                    ""Importance"": 2
                  }},
                  {{
                    ""Word"": ""conveying"",
                    ""Translation"": ""transmitir"",
                    ""Importance"": 2
                  }},
                  {{
                    ""Word"": ""a"",
                    ""Translation"": ""una"",
                    ""Importance"": 3
                  }}
                ]

                Rank the words of the sentence: {0}", text, languageFrom, languageTo);

            return prompt;
        }
    }
}
