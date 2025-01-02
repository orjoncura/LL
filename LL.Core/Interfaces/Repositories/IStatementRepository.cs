
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;

public interface IStatementRepository
{
    int Insert(int seminarWordId, string original, string translated, int userId);
    List<int> InsertRange(int seminarWordId, List<ExerciseShort> statementShorts, int userId);
}

