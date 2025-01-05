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

        public static string CreateCourseWordsPrompt(string text, int languageToId)
        {
            string languageTo = Enum.GetName(typeof(LanguageEnum), languageToId) ?? string.Empty;

            string prompt = string.Format(@"Rank the following word by their importance in the sentence. 
                By importance i mean, how important they are to understand the given sentence in the given context.
                all the words need to be in {1}, If the word is in different language, explicit or isn't understandable, just ignore it.
                Return a ONLY a JSON file and nothing else.
                For examole if you receive the sentence: 'a set of words that is complete in itself, 
                typically containing a subject and predicate, conveying a statement,
                 question, exclamation, or command, and consisting of 
                a main clause and sometimes one or more subordinate clauses.'
                sentence, complete, subject, predicate, statement, question, exclamation, command, clause, subordinate will be marked as '1'
                typically, conveying, main, set, itself, consisting will be marked as '2'
                a, of, in, teh, and, is, that will be marked as '3'

                The JSON file should have the following format:

                [
                  {{
                    ""Word"": ""sentence"",
                    ""Importance"": 1
                  }},
                  {{
                    ""Word"": ""typically"",
                    ""Importance"": 2
                  }},
                  {{
                    ""Word"": ""conveying"",
                    ""Importance"": 2
                  }},
                  {{
                    ""Word"": ""a"",
                    ""Importance"": 3
                  }}
                ]

                Rank the words of the sentence {0}", text, languageTo);

            return prompt;
        }
    }
}
