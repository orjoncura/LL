using LL.Core.Enums;

namespace LL.Core.Factories
{
    public static class PromptFactory
    {
        public static string CreateCoursePrompt(string text, int languageFromId, int languageToId, int numberOfSentences = 10)
        {
            string languageFrom = Enum.GetName(typeof(LanguageEnum), languageFromId) ?? string.Empty;
            string languageTo = Enum.GetName(typeof(LanguageEnum), languageToId) ?? string.Empty;

            string prompt = string.Format(@"Create {3} sentences in {1} for the word:{0} 
                and return a JSON file that contains the new statement and its translation to {2}
                If the word is in different language, explicit or isn't understandable, just ignore it.
                Please make sure what you return is appropriate for kids.
                Please try to use the words below as much as possible.
                The words are {0}
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
                ]", text, languageFrom, languageTo, numberOfSentences);

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
                
               For Translation:

               Return only a JSON file with the ranking and translations.
               Do not translate it if it is a name of a person or a name of organisations and do not include special characters.
               If a word is part of another language (e.g., 'su vida'), split it into individual words ('su' and 'vida') unless separating them changes meaning (e.g., 'otra vez' remains together).
               Provide translations to {2} for each ranked word in the context they appear.
               Example:

               For ranking:

               Words critical to the mai n meaning of the sentence (e.g., nouns, verbs, key concepts) are ranked as '1'.
               Less important words (e.g., adverbs, prepositions) are ranked as '2'.
               Function words (e.g., articles, conjunctions) are ranked as '3'.
               Additionally:

               Your response needs to always include a json even is empty and all the fields need to have a valid value.
               At the end review the json and make sure the translation is correct and it is not a name of a person/organization.
               Also make sure that the 'Definition' and 'PartOfSpeech' are correct. If they are no included at all, please correct that

               The JSON file should have the following format:

               [
                 {{
                   ""Word"": ""sentence"",
                   ""Translation"": ""oración"",
                   ""Definition"": ""The decision or judgement of a jury or court; a verdict."",
                    ""PartOfSpeech"": ""noun"",
                   ""Importance"": 1
                 }},
                 {{
                   ""Word"": ""typically"",
                   ""Translation"": ""típicamente"",                   
                   ""Definition"": ""In a typical or common manner."",
                   ""PartOfSpeech"": ""adverb"",
                   ""Importance"": 2
                 }},
                 {{
                   ""Word"": ""conveying"",
                   ""Translation"": ""transmitir"",                   
                   ""Definition"": ""To move (something) from one place to another."",
                   ""PartOfSpeech"": ""verb"",
                   ""Importance"": 2
                 }},
                 {{
                   ""Word"": ""a"",
                   ""Translation"": ""una"",
                   ""Definition"": ""used when referring to someone or something for the first time in a text or conversation.."",
                   ""PartOfSpeech"": ""Determiner"",
                   ""Importance"": 3
                 }}
               ]

               Rank the words of the sentence: {0}", text, languageFrom, languageTo);

            return prompt;
        }
        public static string CreateDefinitionPrompt(string text)
        {
            string prompt = string.Format
            (@" Create a json based on the example below with the following format for the word: :'{0}'

              1. **Identify Parts of Speech**: Determine if the word has multiple parts of speech (e.g., noun, verb).

              2. **List Definitions**: For each part of speech, list definitions. Each definition should include:
                 - A ""definition"" field.
                 - Optional ""synonyms.""
                 - Optional ""antonyms.""
                 - Optional ""example.""

              3. **Include Synonyms and Antonyms**: At the top level of the JSON, include arrays for global synonyms and antonyms.

              4. **Format Properly**: Ensure proper JSON formatting with commas and brackets to avoid syntax errors.

              5. **Omission of Empty Fields**: If there are no synonyms or antonyms for a definition, use empty arrays instead of omitting the fields.

              6. **Validation**: After generating the JSON, validate it to ensure there are no syntax errors.
          
              Your response needs to always include a json even is empty.
  
                [
                  {{
                    ""partOfSpeech"": ""noun"",
                    ""definitions"": [
                      {{
                        ""definition"": ""A play, dance, or other entertainment."",
                        ""synonyms"": [],
                        ""antonyms"": []
                      }},
                      {{
                        ""definition"": ""An exhibition of items."",
                        ""synonyms"": [],
                        ""antonyms"": [],
                        ""example"": ""art show;  dog show""
                      }},
                      {{
                        ""definition"": ""A broadcast program/programme."",
                        ""synonyms"": [],
                        ""antonyms"": [],
                        ""example"": ""radio show;  television show""
                      }},
                      {{
                        ""definition"": ""A movie."",
                        ""synonyms"": [],
                        ""antonyms"": [],
                        ""example"": ""Let's catch a show.""
                      }},
                      {{
                        ""definition"": ""An agricultural show."",
                        ""synonyms"": [],
                        ""antonyms"": [],
                        ""example"": ""I'm taking the kids to the show on Tuesday.""
                      }},
                      {{
                        ""definition"": ""A project or presentation."",
                        ""synonyms"": [],
                        ""antonyms"": [],
                        ""example"": ""Let's get on with the show.   Let's get this show on the road.   They went on an international road show to sell the shares to investors.   It was Apple's usual dog and pony show.""
                      }},
                      {{
                        ""definition"": ""A demonstration."",
                        ""synonyms"": [],
                        ""antonyms"": [],
                        ""example"": ""show of force""
                      }},
                      {{
                        ""definition"": ""Mere display or pomp with no substance. (Usually seen in the phrases \""all show\"" and \""for show\"".)"",
                        ""synonyms"": [],
                        ""antonyms"": [],
                        ""example"": ""The dog sounds ferocious but it's all show.""
                      }},
                      {{
                        ""definition"": ""Outward appearance; wileful or deceptive appearance."",
                        ""synonyms"": [],
                        ""antonyms"": []
                      }},
                      {{
                        ""definition"": ""(with \""the\"") The major leagues."",
                        ""synonyms"": [],
                        ""antonyms"": [],
                        ""example"": ""He played AA ball for years, but never made it to the show.""
                      }},
                      {{
                        ""definition"": ""A pale blue flame at the top of a candle flame, indicating the presence of firedamp."",
                        ""synonyms"": [],
                        ""antonyms"": []
                      }},
                      {{
                        ""definition"": ""Pretence."",
                        ""synonyms"": [],
                        ""antonyms"": []
                      }},
                      {{
                        ""definition"": ""Sign, token, or indication."",
                        ""synonyms"": [],
                        ""antonyms"": []
                      }},
                      {{
                        ""definition"": ""Semblance; likeness; appearance."",
                        ""synonyms"": [],
                        ""antonyms"": []
                      }},
                      {{
                        ""definition"": ""Plausibility."",
                        ""synonyms"": [],
                        ""antonyms"": []
                      }},
                      {{
                        ""definition"": ""A discharge, from the vagina, of mucus streaked with blood, occurring a short time before labor."",
                        ""synonyms"": [],
                        ""antonyms"": []
                      }}
                    ],
                    ""synonyms"": [
                      ""big leagues"",
                      ""program(me)"",
                      ""demonstration"",
                      ""illustration"",
                      ""proof"",
                      ""exhibition"",
                      ""exposition"",
                      ""façade"",
                      ""front"",
                      ""superficiality""
                    ],
                    ""antonyms"": []
                  }},
                  {{
                    ""partOfSpeech"": ""verb"",
                    ""definitions"": [
                      {{
                        ""definition"": ""To display, to have somebody see (something)."",
                        ""synonyms"": [],
                        ""antonyms"": [],
                        ""example"": ""All he had to show for four years of attendance at college was a framed piece of paper.""
                      }},
                      {{
                        ""definition"": ""To bestow; to confer."",
                        ""synonyms"": [],
                        ""antonyms"": []
                      }},
                      {{
                        ""definition"": ""To indicate (a fact) to be true; to demonstrate."",
                        ""synonyms"": [],
                        ""antonyms"": []
                      }},
                      {{
                        ""definition"": ""To guide or escort."",
                        ""synonyms"": [],
                        ""antonyms"": [],
                        ""example"": ""Could you please show him on his way. He has overstayed his welcome.""
                      }},
                      {{
                        ""definition"": ""To be visible; to be seen; to appear."",
                        ""synonyms"": [],
                        ""antonyms"": [],
                        ""example"": ""At length, his gloom showed.""
                      }},
                      {{
                        ""definition"": ""To put in an appearance; show up."",
                        ""synonyms"": [],
                        ""antonyms"": [],
                        ""example"": ""We waited for an hour, but they never showed.""
                      }},
                      {{
                        ""definition"": ""To have an enlarged belly and thus be recognizable as pregnant."",
                        ""synonyms"": [],
                        ""antonyms"": []
                      }},
                      {{
                        ""definition"": ""(racing) To finish third, especially of horses or dogs."",
                        ""synonyms"": [],
                        ""antonyms"": [],
                        ""example"": ""In the third race: Aces Up won, paying eight dollars; Blarney Stone placed, paying three dollars; and Cinnamon showed, paying five dollars.""
                      }},
                      {{
                        ""definition"": ""To have a certain appearance, such as well or ill, fit or unfit; to become or suit; to appear."",
                        ""synonyms"": [],
                        ""antonyms"": []
                      }}
                    ],
                    ""synonyms"": [
                      ""display"",
                      ""exhibit"",
                      ""indicate"",
                      ""point out"",
                      ""reveal"",
                      ""demonstrate"",
                      ""prove"",
                      ""arrive"",
                      ""show up""
                    ],
                    ""antonyms"": [
                      ""conceal"",
                      ""cover up"",
                      ""hide"",
                      ""disprove"",
                      ""refute""
                    ]
                  }}
                    ]", text);

            return prompt;
        }
    }
}
