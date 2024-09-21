using LL.Core.Models.Short;

namespace LL.Core.Interfaces.Extensions;

public interface IDictionaryService
{
    Task<List<MeaningShort>> GetWordDetails(string word);
}