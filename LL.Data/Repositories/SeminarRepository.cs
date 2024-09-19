using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class SeminarRepository(AppDBContext appDBContext) : ISeminarRepository
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

        appDBContext.Add(seminar);
        appDBContext.SaveChanges();

        return seminar.Id;
    }
}