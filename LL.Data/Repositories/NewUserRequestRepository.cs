using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class NewUserRequestRepository(AppDBContext db) : INewUserRequestRepository
{
    public bool HasReachedLimit(string email, DateTimeOffset date, int attemptsLimit) => 
        db.NewUserRequests.Count(u => 
            u.Email == email 
            && u.CreatedDate.Date.Year == date.Year
            && u.CreatedDate.Date.Month == date.Month
            && u.CreatedDate.Date.Day == date.Day) > attemptsLimit;

    public bool IsTokenValid(Guid token) => 
        db.NewUserRequests.Any(u => u.Token == token) == false;
    
    public string GetEmailByToken(string token) =>
        db.NewUserRequests.SingleOrDefault(u => token.CompareTo(u.Token) == 0).Email;
    
    public int Insert(string email, string ip, Guid token, int loginId)
    {
        var newUserRequest = new NewUserRequest()
        {
            Email = email,
            IP = ip,
            Token = token,
            CreatedById = loginId,
            CreatedDate = DateTime.Now,
        };

        db.Add(newUserRequest);
        db.SaveChanges();

        return newUserRequest.Id;
    }
}