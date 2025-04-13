using LL.Resources.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Resources.Models;

namespace LL.Resources.Repositories;

public class CourseWordRepository(AppDbContext db) : ICourseWordRepository
{
    public int Insert(int wordId, int seminarId, int importanceRatingId, int userId)
    {
        CourseWord? courseWord = db.CourseWords.FirstOrDefault(c => c.WordId == wordId && c.CourseId == seminarId && c.IsActive);

        if (courseWord == null)
        {
            courseWord = new CourseWord()
            {
                WordId = wordId,
                CourseId = seminarId,
                ImportanceRatingId = importanceRatingId,
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