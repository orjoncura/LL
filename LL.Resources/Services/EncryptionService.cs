using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LL.Core.Constants;
using LL.Core.Interfaces.Extensions;
using LL.Core.Model.DataTransferObjects;
using LL.Core.Models.ViewModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace LL.Resources.Services;

public class EncryptionService(IConfiguration configuration) : IEncryptionService
{
    public HashPasswordModel HashPassword(string password)
    {
        // divide by 8 to convert bits to bytes
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); 
        
        return new HashPasswordModel(Hash(password, salt), Convert.ToBase64String(salt));
    }
    public TokenViewModel GenerateAuthenticationToken(int id, string email, TokenConfigModel token)
    {
        var expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(token.Expires));
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Key ?? string.Empty));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = GetClaims(id, email);

        var securityToken = new JwtSecurityToken(
            token.Issuer,
            token.Audience,
            claims,
            expires: expires,
            signingCredentials: credentials);

        return new TokenViewModel()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
            Expiration = expires.ToString(),
        };
    }
    public bool VerifyPassword(string password, string hashedPassword, string salt) =>
        Hash(password, Convert.FromBase64String(salt)) == hashedPassword;
    public string GenerateSecureToken(int length = 64)
    {
        var randomNumber = new byte[length];
        
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        
        return Convert.ToBase64String(randomNumber)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }
    public string Encrypt(int number)
    {
        string encryptionKey = configuration[Secrets.EncryptionKey];
        int half = encryptionKey.Length / 2;
        string key = encryptionKey.Substring(0, half);
        string iv = encryptionKey.Substring(half);
        
        byte[] plainBytes = BitConverter.GetBytes(number);
        using var aes = Aes.Create();
        aes.Key = Encoding.ASCII.GetBytes(key);
        aes.IV = Encoding.ASCII.GetBytes(iv);

        using var encryptor = aes.CreateEncryptor();
        byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        return Convert.ToBase64String(encryptedBytes).Replace("+", "-").Replace("/", "_"); 
    }
    public int Decrypt(string encrypted)
    {               
        string encryptionKey = configuration[Secrets.EncryptionKey];
        int half = encryptionKey.Length / 2;
        string key = encryptionKey.Substring(0, half);
        string iv = encryptionKey.Substring(half);
        
        byte[] encryptedBytes = Convert.FromBase64String(encrypted.Replace("-", "+").Replace("_", "/"));
        using var aes = Aes.Create();
        aes.Key = Encoding.ASCII.GetBytes(key);
        aes.IV = Encoding.ASCII.GetBytes(iv);

        using var decryptor = aes.CreateDecryptor();
        byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
        return BitConverter.ToInt32(decryptedBytes, 0);
    }
    
    // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
    private string Hash(string password, byte[] salt) =>
        Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
    private Claim[] GetClaims(int id, string email)
    {
        return
        [
            new Claim("UserId", id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, email ?? string.Empty)
        ];
    }
}