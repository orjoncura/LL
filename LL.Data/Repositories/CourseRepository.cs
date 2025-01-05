using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class CourseRepository(AppDBContext db) : ICourseRepository
{
    public int Insert(string value, int fromId, int toId, int userId)
    {
        Seminar? course = db.Courses.FirstOrDefault(c => c.Value == value && c.LanguageFromId == fromId && c.LanguageToId == toId && c.IsActive);

        if (course == null)
        {
            course = new Seminar()
            {
                Value = value,
                LanguageFromId = fromId,
                LanguageToId = toId,
                IsActive = true,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            db.Add(course);
            db.SaveChanges();
        }
        
        return course.Id;
    }
}