using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.Arguments;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModel;
using LL.Core.Models.ViewModels;
using LL.Data.Contexts;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class UserRepository(AppDBContext db, IEncryptionService encryptionService) : IUserRepository
{
    public UserShort? GetById(int id) =>
        db.Users
            .Where(u => u.Id == id && u.IsActive)
            .Select(u => new UserShort(u.Id, u.Email, u.PasswordHash))
            .FirstOrDefault();
    
     public UserShort? GetByEmail(string email) =>
         Get(email) is var u && u != null 
             ? new UserShort(u.Id, u.Email, u.PasswordHash)
             : null;

     public TokenViewModel? GetAuthenticationToken(LoginModel loginModel, TokenConfigModel tokenConfigModel)
     {
        if(loginModel.IsValid == false)
            return null;
         
        var user = Get(loginModel.Email);
         
        if(user == null)
            return null;
        
        if(encryptionService.VerifyPassword(loginModel.Password, user.PasswordHash, user.Salt) == false)
            return null;

        var tokenViewModel = encryptionService.GenerateAuthenticationToken(user.Id, user.Email, tokenConfigModel);

        if(tokenViewModel == null || string.IsNullOrWhiteSpace(tokenViewModel.Token))
            return null;

        var userToken = new UserToken()
        {
            Token = tokenViewModel.Token,
            Expiration = DateTime.UtcNow.AddMinutes(Convert.ToInt32(tokenConfigModel.Expires)),
            UserId = user.Id
        };

        db.Add(userToken);
        db.SaveChanges();

        return tokenViewModel;
     }
     
     private User? Get(string email) =>
         db.Users.FirstOrDefault(u =>
             string.IsNullOrWhiteSpace(u.Email) == false
             && u.Email.ToLower().Trim() == email.ToLower().Trim() 
             && u.IsActive);
     
     public int Insert(string email, string password, int loginId)
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
             CreatedById = loginId,
             CreatedDate = new DateTimeOffset()
         };

         db.Add(userLog);
         db.SaveChanges();
         
         return user.Id;
     }

     public bool UpdatePassword(int userId, string password, int loginId)
     {
         User? user = db.Users.FirstOrDefault(u => u.Id == userId);         
         
         if (user == null) 
             return false;
         
         user.PasswordHash = password;
         
         db.Add(user);
         db.SaveChanges();
         
         var userLog = new UserLog()
         {
             UserId = user.Id,
             Email = user.Email,
             PasswordHash = user.PasswordHash,
             IsActive = user.IsActive,
             CreatedById = loginId,
             CreatedDate = new DateTimeOffset()
         };

         db.Add(userLog);
         db.SaveChanges();
         
         return user.PasswordHash == password;
     }
}