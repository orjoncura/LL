using LL.Core.Models.ViewModel;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Extensions;

public interface IEncryptionService
{
    Dictionary<string, string> HashPassword(string password);

    bool VerifyPassword(string password, string hashedPassword, string salt);

    string GenerateSecureToken(int length = 64);

    TokenViewModel GenerateAuthenticationToken(int id, string email, TokenConfigModel token);
}