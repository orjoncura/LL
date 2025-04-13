using LL.Resources.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Resources.Models;

namespace LL.Resources.Repositories;

public class CourseRepository(AppDbContext db) : ICourseRepository
{
    public int Insert(string value, int fromId, int toId, int userId)
    {
        Course? course = db.Courses.FirstOrDefault(c => c.Value == value && c.LanguageFromId == fromId && c.LanguageToId == toId && c.IsActive);

        if (course == null)
        {
            course = new Course()
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