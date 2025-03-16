using DataBase.Data;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using TaskManagementAPI.Models;

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
                          .Substring(0, 15);
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


        // hàm upload file
        public static async Task<string> UploadFile(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
                return null;

            // Định dạng tên file duy nhất
            string fileName =file.FileName;
            string fullPath = Path.Combine(folderPath, fileName);

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Lưu file vào thư mục
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName; // Trả về tên file để lưu vào database
        }

        // lấy địa chỉ host
        public static string GetHostUrl(IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            var request = contextAccessor.HttpContext?.Request;
            return request != null ? $"{request.Scheme}://{request.Host}" : configuration["STMP:Domain"];
        }


        // tự tăng cột theo index hiện tại
        public static int GetNextAutoIncrementValue <T>(ApplicationDbContext dbContext, Expression<Func<T, int?>> fieldSelector) where T : class
        {
            // Lấy giá trị lớn nhất của cột được chỉ định
            int? maxCode = dbContext.Set<T>().Max(fieldSelector);

            // Nếu chưa có mã nào, trả về 1; nếu có, tăng lên 1
            return (maxCode ?? 0) + 1;
        }

    }
}
