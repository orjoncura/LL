using LL.Data.Contexts;
using LL.Data.Interfaces;
using LL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.Data.Repositories
{
    public class SecurityRepository(AppDBContext appDBContext) : ISecurityRepository
    {
        public User? GetLoginByUsername(string username)
        {
            return appDBContext.Users
                .Where(u => u.Username.ToLower().Trim() == username.ToLower().Trim() && u.IsActive).FirstOrDefault();
        }
    }
}
