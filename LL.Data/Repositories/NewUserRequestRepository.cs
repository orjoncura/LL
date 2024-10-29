using LL.Core.Interfaces.Extensions;
using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class NewUserRequestRepository(AppDBContext db, IEncryptionService encryptionService) : INewUserRequestRepository
{
    public bool HasReachedLimit(string email, DateTimeOffset date, int attemptsLimit) => 
        db.NewUserRequests.Count(u => 
            u.Email == email 
            && u.CreatedDate.Date.Year == date.Year
            && u.CreatedDate.Date.Month == date.Month
            && u.CreatedDate.Date.Day == date.Day) > attemptsLimit;
    
    public string? GetEmailByToken(string token) =>
        db.NewUserRequests.FirstOrDefault(u => u.Token == token)?.Email;
    
    public string Insert(string email, string ip, int loginId)
    {
        if (string.IsNullOrWhiteSpace(email))
            return string.Empty;
        
        var newUserRequest = new NewUserRequest()
        {
            Email = email.Trim().ToLower(),
            IP = ip,
            Token = encryptionService.GenerateSecureToken(),
            CreatedById = loginId,
            CreatedDate = DateTimeOffset.Now,
        };

        db.Add(newUserRequest);
        db.SaveChanges();

        return newUserRequest.Id > 0 ? newUserRequest.Token : string.Empty;
    }
}