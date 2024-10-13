using System.Security.Cryptography;

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

