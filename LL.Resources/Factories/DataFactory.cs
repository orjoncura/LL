using LL.Core.Enums;
using LL.Core.Helpers;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;
using LL.Resources.Models;

namespace LL.Resources.Factories;

public static class DataFactory
{
    public static WordShort Convert(Word word) =>
        new()
        {
            Id = word.Id,
            Name = word.Name,
            Language = EnumHelper.GetEnumValueById<LanguageEnum>(word.LanguageId),
            ImportanceRatingId = word.ImportanceRatingId
        };
    public static CourseViewModel Convert(Course course) =>
        new()
        {
            Id = course.Id,
            Text = course.Value,
            CreatedDate = new DateTimeViewModel(course.CreatedDate) 
        };
    public static ExerciseViewModel Convert(Exercise exercise) =>
        new()
        {
            Id = exercise.Id,
            Original = exercise.Original,
            Translated = exercise.Translated,
            Extra = exercise.Extra
        };
    public static MeaningShort Convert(WordMeaning wordMeaning) =>
        new()
        {
            Type = wordMeaning.Type.Value,
            Definitions = wordMeaning.WordDefinitions.Select(w => w.Value).ToList()
        };
}