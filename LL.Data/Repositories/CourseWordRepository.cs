using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class CourseWordRepository(AppDbContext db) : ICourseWordRepository
{
    public int Insert(int wordId, int seminarId, int seminarWordRankId, int userId)
    {
        CourseWord? courseWord = db.CourseWords.FirstOrDefault(c => c.WordId == wordId && c.CourseId == seminarId && c.IsActive);

        if (courseWord == null)
        {
            courseWord = new CourseWord()
            {
                WordId = wordId,
                CourseId = seminarId,
                CourseWordRankId = seminarWordRankId,
                IsActive = true,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };
            
            db.Add(courseWord);
            db.SaveChanges();
        }
        
        return courseWord.Id;
    }
}