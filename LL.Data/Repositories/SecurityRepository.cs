// using LL.Data.Contexts;
// using LL.Core.Interfaces.Repositories;
// using LL.Data.Model;
//
// namespace LL.Data.Repositories;
//
// public class SecurityRepository(AppDBContext appDBContext) : ISecurityRepository
// {
//     public User? GetLoginByUsername(string username) =>
//         appDBContext.Users.FirstOrDefault(u => 
//                 u.Username.ToLower().Trim() == username.ToLower().Trim() 
//                 && u.IsActive);
// }
//
