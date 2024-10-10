using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class NewUserRequestRepository(AppDBContext db) : INewUserRequestRepository
{
    public bool HasReachedLimit(int userId, DateTimeOffset date, int attemptsLimit) => 
        db.NewUserRequests.Count(u => 
            u.UserId == userId 
            && u.CreatedDate.Date.Year == date.Year
            && u.CreatedDate.Date.Month == date.Month
            && u.CreatedDate.Date.Day == date.Day) > attemptsLimit;

    public int Insert(int userId, string ip, Guid token)
    {
        var newUserRequest = new NewUserRequest()
        {
            UserId = userId,
            IP = ip,
            Token = token,
            CreatedDate = DateTime.Now,
        };

        db.Add(newUserRequest);
        db.SaveChanges();

        return newUserRequest.Id;
    }
}