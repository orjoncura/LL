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

        public static string CreateSeminarWordsPrompt(string text, int languageToId)
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
