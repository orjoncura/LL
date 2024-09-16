using LL.Data.Contexts;
using LL.Data.Interfaces;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class SeminarRepository(AppDBContext appDBContext) : ISeminarRepository
{
    public int Insert(string value, int userId)
    {
        var seminar = new Seminar()
        {
            Value = value,
            IsActive = true,
            CreatedById = userId,
            CreatedDate = DateTime.Now,
        };

        appDBContext.Add(seminar);
        appDBContext.SaveChanges();

        return seminar.Id;
    }
}