using LL.Data.Contexts;
using LL.Data.Interfaces;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class SeminarWordRankRepository(AppDBContext appDBContext) : ISeminarWordRankRepository
{
    public int Insert(string name, int userId)
    {
        var seminarWordRank = new SeminarWordRank()
        {
            Name = name,
            IsActive = true,
            CreatedById = userId,
            CreatedDate = DateTime.Now,
        };

        appDBContext.Add(seminarWordRank);
        appDBContext.SaveChanges();

        return seminarWordRank.Id;
    }
}