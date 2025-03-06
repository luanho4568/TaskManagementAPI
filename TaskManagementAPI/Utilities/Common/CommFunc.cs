using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Security.Cryptography;
using System.Text;

namespace TaskManagementAPI.Utilities.Common
{
    public static class CommFunc
    {
        public static string NewShortId()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                          .Replace("=", "")
                          .Replace("/", "")
                          .Replace("+", "")
                          .Substring(0, 12);
        }

        // Hash password với độ phức tạp (work factor) là 12
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        // Kiểm tra mật khẩu nhập vào có đúng với hash không
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
