using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class Users
    {
        [Key]
        public string? Id { get; set; }
        public string? Name { get; set; }
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string? Email { get; set; }
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự!")]
        public string? Password { get; set; }
        public string? HashPassword { get; set; }
        public string? Role { get; set; } // admin và user
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public int? PersonalLink { get; set; } // Link cá nhân
        public string? Type { get; set; } // phương thức đăng nhập
        public string? Bio { get; set; } // giới thiệu
        public string? Status { get; set; } // online off
        public int? Login_attempts { get; set; } // kiểm tra lần sai
        public DateTime? Lockout_EndTime { get; set; } // lưu thời gian khoá

        public DateTime? Last_login { get; set; } // để lấy lần đăng nhập gần nhất
        public DateTime? Status_login { get; set; } // để lấy trạng thái đăng nhập gần nhất khi off
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
    }
}
