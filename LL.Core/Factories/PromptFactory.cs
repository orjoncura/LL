namespace LL.Core.Factories
{
    public static class PromptFactory
    {
        public static string CreateSeminarPrompt(string word)
        {
            string prompt = string.Format(@"Create 3 sentences in Spanish for the word:{0} 
                and return a JSON file that contains the word, the sentences, and their translation to English.
                The JSON file should have the following format:

                {
                    ""TargetWord"": {
                        ""OriginalStatement"": ""{0}"",
                        ""TranslatedStatement"": ""ejemplo"",
                    },
                    ""Sentence"": [
                        {
                            ""OriginalStatement"": ""This is an example with the word {0}."",
                            ""TranslatedStatement"": ""Esta es una oraci�n de ejemplo."",
                        },
                        {
                            ""OriginalStatement"": ""Another example with the word {0}."",
                            ""TranslatedStatement"": ""Otra oraci�n de ejemplo."",
                        },
                        {
                            ""OriginalStatement"": ""A third example with the word {0}."",
                            ""TranslatedStatement"": ""Otra oraci�n de ejemplo."",
                        }
                    ]
                }", word);

            return prompt;
        }
    }
}
