using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class CourseRepository(AppDBContext db) : ICourseRepository
{
    public int Insert(string value, int fromId, int toId, int userId)
    {
        var seminar = new Seminar()
        {
            Value = value,
            LanguageFromId = fromId,
            LanguageToId = toId,
            IsActive = true,
            CreatedById = userId,
            CreatedDate = DateTime.Now,
        };

        db.Add(seminar);
        db.SaveChanges();

        return seminar.Id;
    }
}