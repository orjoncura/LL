using LL.Data.Contexts;
using LL.Data.Interfaces;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class SeminarWordRepository(AppDBContext appDBContext) : ISeminarWordRepository
{
    public int Insert(int wordId, int seminarId, int seminarWordRankId, int userId)
    {
        var seminarWord = new SeminarWord()
        {
            WordId = wordId,
            SeminarId = seminarId,
            SeminarWordRankId = seminarWordRankId,
            IsActive = true,
            CreatedById = userId,
            CreatedDate = DateTime.Now,
        };

        appDBContext.Add(seminarWord);
        appDBContext.SaveChanges();

        return seminarWord.Id;
    }
    
    public bool InsertRange(List<SeminarWord> seminarWords)
    {
        seminarWords = seminarWords.Where(s => 
            s.WordId > 0 
            & s.SeminarWordRankId > 0
            & s.SeminarId > 0 ).ToList();
        
        appDBContext.AddRange(seminarWords);
        appDBContext.SaveChanges();

        return seminarWords.All(s => s.Id > 0);
    }
}