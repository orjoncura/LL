using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Data.Model;

public class ResetPasswordRequestRepository(AppDBContext db) : IResetPasswordRequestRepository
{
    public bool HasReachedLimit(int userId, DateTimeOffset date, int attemptsLimit) => 
        db.ResetPasswordRequests.Count(u => 
            u.UserId == userId 
            && u.CreatedDate.Date.Year == date.Year
            && u.CreatedDate.Date.Month == date.Month
            && u.CreatedDate.Date.Day == date.Day) > attemptsLimit;

    public bool IsTokenValid(string token) => 
        db.ResetPasswordRequests.Any(u => u.Token == token) == false;
    
    public int GetUserIdByToken(string token) =>
        db.ResetPasswordRequests.SingleOrDefault(u => u.Token == token).UserId;
    
    public int Insert(int userId,string ip, string token)
    {
        var resetPasswordRequests = new ResetPasswordRequest()
        {
            UserId = userId,
            IP = ip,
            Token = token,
            CreatedDate = DateTime.Now,
        };

        db.Add(resetPasswordRequests);
        db.SaveChanges();

        return resetPasswordRequests.Id;
    }
}