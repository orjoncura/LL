using LL.Core.Models.Short;

namespace LL.Core.Interfaces.Repositories;

public interface ICourseWordRepository
{ 
    int Insert(int wordId, int moduleId, int seminarWordRankId, int userId);
}