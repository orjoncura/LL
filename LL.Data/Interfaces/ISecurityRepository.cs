using LL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.Data.Interfaces
{
    public interface ISecurityRepository
    {
        public User? GetLoginByUsername(string username);
    }
}
