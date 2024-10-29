using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace LL.Core.Helpers;

public static class SecurityHelper
{    
    public static bool VerifyHashedPassword(string hashedPassword, string password)
    {
        ArgumentNullException.ThrowIfNull(hashedPassword);
        ArgumentNullException.ThrowIfNull(password);

        byte[] array = Convert.FromBase64String(hashedPassword);

        if (array.Length != 49 || array[0] != 0)
        {
            return false;
        }

        byte[] array2 = new byte[16];
        Buffer.BlockCopy(array, 1, array2, 0, 16);
        byte[] array3 = new byte[32];
        Buffer.BlockCopy(array, 17, array3, 0, 32);
        byte[] bytes;
        using (Rfc2898DeriveBytes rfc2898DeriveBytes = new(password, array2, 34452, HashAlgorithmName.SHA1))
        {
            bytes = rfc2898DeriveBytes.GetBytes(32);
        }

        return ByteArraysEqual(array3, bytes);
    }
    public static string HashPassword(string password)
    {
        // divide by 8 to convert bits to bytes
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); 

        // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return hashed;
    }
    public static string GenerateSecureToken(int length = 64)
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
    private static bool ByteArraysEqual(byte[] a, byte[] b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (a == null || b == null || a.Length != b.Length)
        {
            return false;
        }

        bool flag = true;
        for (int i = 0; i < a.Length; i++)
        {
            flag &= a[i] == b[i];
        }

        return flag;
    }
}

