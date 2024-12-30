using LL.Core.Enums;
using LL.Core.Helpers;
using LL.Core.Models.Short;
using LL.Data.Model;

namespace LL.Data.Factories;

public static class DataFactory
{
    public static WordShort Convert(Word word)
    {
        return new WordShort()
        {
            Id = word.Id,
            Name = word.Name,
            Language = EnumHelper.GetEnumValueById<LanguageEnum>(word.LanguageId)
        };
    }
}