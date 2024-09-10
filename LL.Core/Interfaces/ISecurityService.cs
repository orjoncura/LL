using LL.Data.Model;
using LL.SharedDefinitions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.Core.Interfaces
{
    public interface ISecurityService
    {
        TokenViewModel? Authenticate(LoginModel loginModel);
    }
}
