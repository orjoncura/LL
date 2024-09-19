namespace LL.Core.Interfaces.Repositories;

public interface ISeminarWordRepository
{ 
    int Insert(int wordId, int seminarId, int seminarWordRankId, int userId);
}