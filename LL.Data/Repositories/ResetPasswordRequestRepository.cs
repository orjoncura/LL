using LL.Core.Interfaces.Extensions;
using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Data.Model;

public class ResetPasswordRequestRepository(AppDbContext db, IEncryptionService encryptionService) : IResetPasswordRequestRepository
{
    public bool HasReachedLimit(int userId, DateTimeOffset date, int attemptsLimit) => 
        db.ResetPasswordRequests.Count(u => 
            u.UserId == userId 
            && u.CreatedDate.Date.Year == date.Year
            && u.CreatedDate.Date.Month == date.Month
            && u.CreatedDate.Date.Day == date.Day) > attemptsLimit;
    
    public int GetUserIdByToken(string token) =>
        db.ResetPasswordRequests.SingleOrDefault(u => u.Token == token).UserId;
    
    public string Insert(int userId, string ip)
    {
        var resetPasswordRequests = new ResetPasswordRequest()
        {
            UserId = userId,
            IP = ip,
            Token = encryptionService.GenerateSecureToken(),
            CreatedDate = DateTime.Now,
        };

        db.Add(resetPasswordRequests);
        db.SaveChanges();

        return resetPasswordRequests.Id > 0 ? resetPasswordRequests.Token : string.Empty;
    }
}