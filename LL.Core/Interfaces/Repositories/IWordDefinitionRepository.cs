namespace LL.Core.Interfaces.Repositories;

public interface IWordDefinitionRepository
{
    int Insert(string definition, int wordMeaningId, int userId);
}