using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class CourseWordRepository(AppDBContext db) : ICourseWordRepository
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

        db.Add(seminarWord);
        db.SaveChanges();

        return seminarWord.Id;
    }
}