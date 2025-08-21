using System.Text.RegularExpressions;
using LL.Core.Enums;
using LL.Core.Factories;
using LL.Core.Helpers;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;

namespace LL.Core.Services;

public class CourseService(
    ICourseRepository courseRepository,
    IModuleRepository moduleRepository,
    IWordRepository wordRepository,
    IWordMeaningRepository wordMeaningRepository,
    IWordDefinitionRepository wordDefinitionRepository,
    IWordLinkRepository wordLinkRepository,
    IExerciseRepository exerciseRepository,
    ICourseWordRepository courseWordRepository,
    IAgentService agentService) : ICourseService
{
    
    public class SentenceGroupResult
    {
        public List<string> Sentences { get; set; }
        public List<string> Words { get; set; }  
    }
    
    public async Task<bool> CreateCourse(CourseRequestModel courseRequest, int userId)
    {
        if(courseRequest.IsValid == false) 
            return false;
        
        //Strip HTML & create the course.
        var sentenceGroupResults = ProcessText(courseRequest.Text);
        int courseId = courseRepository.Insert(courseRequest.Text, courseRequest.LanguageFromId, courseRequest.LanguageToId, userId);

        //Get words that already exists in the database.
        var words = sentenceGroupResults.SelectMany(w => w.Words).ToList();
        var dbWords = wordRepository.GetRangeByText(words, courseRequest.LanguageFromId);
        
        //Create the first module with the keywords.
        CreateKeyWordModule(dbWords, courseId, userId);
        
        int index = 1;
        int sequence = 2;
        foreach (var sentenceGroupResult in sentenceGroupResults)
        {            
            string moduleTitleSuffix = ": Part " + index;
            
            await CreateFlashcardModule(courseId,
                sentenceGroupResult.Words,
                dbWords,
                moduleTitleSuffix,
                sequence,
                courseRequest.LanguageFromId,
                courseRequest.LanguageToId,
                userId);
            sequence++;

            await CreateExerciseModule(courseId,
                sentenceGroupResult.Sentences, 
                moduleTitleSuffix, 
                sequence, 
                courseRequest,
                userId);
            sequence++;
            
            index++;
        }
        
        return true;
    }
    
    private List<SentenceGroupResult> ProcessText(string htmlText)
    {
        //Strip HTML
        string plainText = Regex.Replace(htmlText, "<.*?>", string.Empty);

        //Extract all unique words in order of first appearance
        var allWordsOrdered = Regex.Matches(plainText.ToLower(), @"\b[\w']+\b")
                                   .Select(m => m.Value)
                                   .ToList();

        var allUniqueWords = new List<string>();
        var seen = new HashSet<string>();
        foreach (var w in allWordsOrdered)
        {
            if (!seen.Contains(w))
            {
                seen.Add(w);
                allUniqueWords.Add(w);
            }
        }
        
        //Split into sentences
        var sentences = Regex.Split(plainText, @"(?<=[\.!\?])\s+")
                             .Where(s => !string.IsNullOrWhiteSpace(s))
                             .ToList();
        
        //Create groups
        var results = new List<SentenceGroupResult>();
        var usedWords = new HashSet<string>();
        var currentSentences = new List<string>();
        var currentWords = new HashSet<string>();

        foreach (var sentence in sentences)
        {
            //Extract unique words from this sentence that are not already used globally
            var sentenceWords = Regex.Matches(sentence.ToLower(), @"\b[\w']+\b")
                                     .Select(m => m.Value)
                                     .Where(w => !usedWords.Contains(w))
                                     .ToList();

            //Add sentence & words
            currentSentences.Add(sentence);
            foreach (var word in sentenceWords)
                currentWords.Add(word);

            //If we hit limits → save group
            if (currentSentences.Count >= 35 || currentWords.Count >= 100)
            {
                if (currentWords.Count >= 30) // only store if we have minimum words
                {
                    results.Add(new SentenceGroupResult
                    {
                        Sentences = new List<string>(currentSentences),
                        Words = currentWords.ToList()
                    });

                    // Mark words as used globally
                    foreach (var w in currentWords)
                        usedWords.Add(w);

                    // Reset for next group
                    currentSentences.Clear();
                    currentWords.Clear();
                }
            }
        }

        //Add remaining sentences if they meet minimum word count
        if (currentSentences.Count > 0 && currentWords.Count >= 30)
        {
            results.Add(new SentenceGroupResult
            {
                Sentences = new List<string>(currentSentences),
                Words = currentWords.ToList()
            });

            foreach (var w in currentWords)
                usedWords.Add(w);
        }

        return results;
    }
    private void CreateKeyWordModule(List<WordViewModel> wordViewModels, int courseId, int userId)
    {
        if (moduleRepository.ModuleCreated(courseId, (int)ModuleTypeEnum.Multiselect)) return;
        
        //Create a multiselect module based on the high priority words in the database.
        var keywords = wordViewModels
            .Where(w => w.ImportanceRatingId == (int)ImportanceRatingEnum.High).ToList();

        if (keywords.Count() > 5)
        {
            int multiselectModuleId = moduleRepository.Insert(courseId, "Key Words", (int)ModuleTypeEnum.Multiselect, 1, true, userId);   
            keywords.ForEach(k => courseWordRepository.Insert(k.Id, multiselectModuleId, (int)ImportanceRatingEnum.High, userId)); 
        }
    }
    private async Task CreateFlashcardModule(int courseId, List<string> words, List<WordViewModel> dbWords, string moduleTitleSuffix, int sequence, int fromId, int toId, int userId)
    {
        var formattedItems = string.Join(",", words.Where(n => dbWords.Any(w => w.Name == n) == false).ToList()
            .Select(item =>$"{item}" ).ToList());
        
        //Create the prompt and ask the agent to create a CourseWordsModel for the new words.
        //Safely extract the list of CourseWordsModel from JSON, that is returned by the agent.
        var prompt = PromptFactory.CreateCourseWordsPrompt(formattedItems, fromId, toId);
        var response = await agentService.Run(prompt);
        var extractedModels = !string.IsNullOrEmpty(response) ? JsonHelper.Extract<List<CourseWordsModel>>(response) ?? [] : [];
        
        int flashcardModuleId = moduleRepository.Insert(courseId, "Flashcards" + moduleTitleSuffix, (int)ModuleTypeEnum.Flashcards, sequence, sequence == 2, userId);

        foreach (var courseWord in extractedModels)
        {
            WordShort wordShort = wordRepository.Insert(courseWord.Word, fromId, userId);

            courseWordRepository.Insert(wordShort.Id, flashcardModuleId, courseWord.Importance, userId);
            wordLinkRepository.Insert(wordShort.Id, courseWord.Translation, fromId, toId, userId);

            MeaningShort meaning = new MeaningShort
            {
                Definitions = new List<string>()
                {
                    courseWord.Definition
                },
                Type = courseWord.PartOfSpeech
            };

            if (wordMeaningRepository.GetByWordId(wordShort.Id) == null)
            {
                if (!Enum.TryParse(meaning.Type, true,
                        out WordTypeEnum parsedValue)) // `true` for case-insensitive parsing
                    continue;

                int wordMeaningId = wordMeaningRepository
                    .Insert(wordShort.Id, (int)parsedValue, userId);

                meaning.Definitions.ForEach(d => wordDefinitionRepository.Insert(d, wordMeaningId, userId));
            }
        }
        
        //Create a course words model for the remaining words, we will skip the keywords because they were inserted earlier.
        dbWords.Where(w => words.Contains(w.Name) && w.ImportanceRatingId != (int)ImportanceRatingEnum.High).ToList()
            .ForEach(w => courseWordRepository.Insert(w.Id, flashcardModuleId, (int)ImportanceRatingEnum.Medium, userId));
    }
    private async Task CreateExerciseModule(int courseId, List<string> sentences, string moduleTitleSufix, int sequence, CourseRequestModel courseRequest, int userId)
    {     
        //Create a new module for the exercises.
        int exercisesModuleId = moduleRepository.Insert(courseId, "Exercises" + moduleTitleSufix, (int)ModuleTypeEnum.Exercises, sequence, false, userId); 
        
        //Create the prompt and ask the agent to create a ExerciseViewModel for the new exercises.
        //Safely extract the list of ExerciseViewModel from JSON, that is returned by the agent.
        var formattedItems = string.Join("\n", sentences.Select(item =>$"{item}" ).ToList());
        string prompt = PromptFactory.CreateExercisesPrompt(formattedItems, courseRequest.LanguageToId);
        var response = await agentService.Run(prompt);
        var exercises = !string.IsNullOrEmpty(response) ? JsonHelper.Extract<List<ExerciseViewModel>>(response) : [];
        
        if (exercises != null && exercises.Any(s => s.IsValid))
            exerciseRepository.InsertRange(exercisesModuleId, exercises.Where(e => e.IsValid).ToList(), userId);
    }
}

