using LL.Core.Enums;
using LL.Core.Helpers;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;
using LL.Data.Model;

namespace LL.Data.Factories;

public static class DataFactory
{
    public static WordShort Convert(Word word) =>
        new()
        {
            Id = word.Id,
            Name = word.Name,
            Language = EnumHelper.GetEnumValueById<LanguageEnum>(word.LanguageId)
        };
    
    public static ExerciseViewModel Convert(Exercise exercise) =>
        new()
        {
            Id = exercise.Id,
            Original = exercise.Original,
            Translated = exercise.Translated,
            Extra = exercise.Extra
        };
}