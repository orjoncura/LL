using LL.Core.Interfaces.Repositories;
using LL.Core.Models.Short;
using LL.Data.Contexts;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class UserRepository(AppDBContext db) : IUserRepository
{
     public UserShort? GetByEmail(string email) =>
         db.Users.Where(u => 
             u.Email.ToLower().Trim() == email.ToLower().Trim() 
             && u.IsActive).Select(u => 
                 new UserShort(u.Id, u.Email, u.PasswordHash))
             .FirstOrDefault();

     private User? Get(string email) =>
         db.Users.FirstOrDefault(u => 
             u.Email.ToLower().Trim() == email.ToLower().Trim());
     
     public int Insert(string email, string password)
     {
         User? user = Get(email);
         
         if (user != null) 
             return user.Id;
         
         user = new User()
         {
             Email = email,
             PasswordHash = password,
             IsActive = true,
         };

         db.Add(user);
         db.SaveChanges();

         var userLog = new UserLog()
         {
             UserId = user.Id,
             Email = email,
             PasswordHash = password,
             IsActive = true,
         };

         db.Add(userLog);
         db.SaveChanges();
         
         return user.Id;
     }
}