using LL.Core.Models.Short;

namespace LL.Core.Interfaces.Repositories;

public interface ICourseWordRepository
{ 
    int Insert(int wordId, int seminarId, int seminarWordRankId, int userId);
}