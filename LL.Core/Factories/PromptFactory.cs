namespace LL.Core.Factories
{
    public static class PromptFactory
    {
        public static string CreateSeminarPrompt(string word)
        {
            string prompt = string.Format(@"Create 3 sentences in Spanish for the word:{0} 
                and return a JSON file that contains the word, the sentences, and their translation to English.
                The JSON file should have the following format:

                {{
                    ""TargetWord"": {{
                        ""OriginalStatement"": ""{0}"",
                        ""TranslatedStatement"": """",
                    }},
                    ""Sentence"": [
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
                }}", word);

            return prompt;
        }
    }
}
