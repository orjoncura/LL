using LL.Core.Models.Short;

namespace LL.Core.Interfaces.Repositories;

public interface ICourseWordRepository
{ 
    void Insert(string wordId, int moduleId, int importanceRatingId, int userId);
    int Insert(int wordId, int moduleId, int importanceRatingId, int userId);
}