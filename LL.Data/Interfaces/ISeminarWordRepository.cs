namespace LL.Data.Interfaces;

public interface ISeminarWordRepository
{ 
    int Insert(int wordId, int seminarId, int seminarWordRankId, int userId);
}