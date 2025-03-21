using LL.Core.Models.Arguments;
using LL.Core.Models.Short;

namespace LL.Core.Interfaces.Repositories;

public interface IWordLinkRepository
{
    WordLinkShort Insert(int wordId, DefinitionRequestModel definitionRequestModel, int userId);
}