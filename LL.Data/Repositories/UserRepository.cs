using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Model.DataTransferObjects;
using LL.Core.Models.Arguments;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;
using LL.Data.Model;
using LL.Data.Contexts;

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

     public TokenViewModel GetAuthenticationToken(LoginModel loginModel, TokenConfigModel tokenConfigModel)
     {
        TokenViewModel tokenViewModel = new TokenViewModel();

        if (loginModel.IsValid == false)
            return tokenViewModel;
         
        var user = Get(loginModel.Email);
         
        if(user == null)
            return tokenViewModel;

        if (encryptionService.VerifyPassword(loginModel.Password, user.PasswordHash, user.Salt) == false)
            return tokenViewModel;

        tokenViewModel = encryptionService.GenerateAuthenticationToken(user.Id, user.Email, tokenConfigModel);

        if(string.IsNullOrWhiteSpace(tokenViewModel.Token))
            return tokenViewModel;

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

        HashPasswordModel hashPasswordModel = encryptionService.HashPassword(password);

         user = new User()
         {
             Email = email,
             PasswordHash = hashPasswordModel.Password,
             Salt = hashPasswordModel.Salt,
             IsActive = true,
         };

         db.Add(user);
         db.SaveChanges();

         var userLog = new UserLog()
         {
             UserId = user.Id,
             Email = email,
             PasswordHash = hashPasswordModel.Password,
             Salt = hashPasswordModel.Salt,
             IsActive = true,
             CreatedById = loginId,
             CreatedDate = DateTimeOffset.Now
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
             CreatedDate = DateTimeOffset.Now
         };

         db.Add(userLog);
         db.SaveChanges();
         
         return user.PasswordHash == password;
     }
}