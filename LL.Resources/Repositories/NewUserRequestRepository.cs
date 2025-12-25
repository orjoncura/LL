using LL.Core.Interfaces.Extensions;
using LL.Resources.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Resources.Models;

namespace LL.Resources.Repositories;

public class NewUserRequestRepository(AppDbContext db, IEncryptionService encryptionService) : INewUserRequestRepository
{
    public bool HasReachedLimit(string email, DateTime date, int attemptsLimit) => 
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
            CreatedDate = DateTime.Now,
        };

        db.Add(newUserRequest);
        db.SaveChanges();

        return newUserRequest.Id > 0 ? newUserRequest.Token : string.Empty;
    }
}