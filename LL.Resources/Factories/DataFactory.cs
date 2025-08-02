using LL.Core.Enums;
using LL.Core.Helpers;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;
using LL.Resources.Models;
using LL.Resources.Services;

namespace LL.Resources.Factories;

public static class DataFactory
{
    public static CourseViewModel Convert(string id, Course course) =>
        new()
        {
            Id = id,
            Text = course.Value,
            CreatedDate = new DateTimeViewModel(course.CreatedDate) 
        };
    
    public static ExerciseViewModel Convert(string id, Exercise exercise) =>
        new()
        {
            Id = id,
            Original = exercise.Original,
            Translated = exercise.Translated,
            Extra = exercise.Extra
        };
    
    public static ModuleViewModel Convert(string id, Module module) =>
        new()
        {
            Id = id,
            Title = module.Title,
            Type = module.Type != null ? module.Type.Value : "",
            Unlocked = module.Unlocked,
            Completed = module.Completed
        };
    
    public static WordShort Convert(Word word) =>
        new()
        {
            Id = word.Id,
            Name = word.Name,
            Language = EnumHelper.GetEnumValueById<LanguageEnum>(word.LanguageId),
            ImportanceRatingId = word.ImportanceRatingId
        };
    
    public static MeaningShort Convert(WordMeaning wordMeaning) =>
        new()
        {
            Type = wordMeaning.Type.Value,
            Definitions = wordMeaning.WordDefinitions.Select(w => w.Value).ToList()
        };
    
    public static WordViewModel Convert(string id, WordLink wl) =>
        new()
        {
            Id = id,
            Name = wl.Word.Name, 
            Translation = wl.Value, 
            ImportanceRatingId = wl.Word.ImportanceRatingId,
            Meanings = wl.Word.WordMeanings.Select(Convert).ToList()
        };
}