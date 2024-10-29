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

    public bool IsTokenValid(string token) => 
        db.NewUserRequests.Any(u => u.Token == token) == false;
    
    public string? GetEmailByToken(string token) =>
        db.NewUserRequests.FirstOrDefault(u => u.Token == token)?.Email;
    
    public int Insert(string email, string ip, string token, int loginId)
    {
        if (string.IsNullOrWhiteSpace(email))
            return 0;
        
        var newUserRequest = new NewUserRequest()
        {
            Email = email.Trim().ToLower(),
            IP = ip,
            Token = token,
            CreatedById = loginId,
            CreatedDate = DateTimeOffset.Now,
        };

        db.Add(newUserRequest);
        db.SaveChanges();

        return newUserRequest.Id;
    }
}