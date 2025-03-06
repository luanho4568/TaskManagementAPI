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
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } // admin và user
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public string? Type { get; set; } // phương thức đăng nhập
        public string? Bio { get; set; } // giới thiệu
        public string? Status { get; set; } // online off
        public string? Login_attempts { get; set; } // kiểm tra lần sai

        public string? Last_login { get; set; } // để lấy lần đăng nhập gần nhất
        public string? Status_login { get; set; } // để lấy trạng thái đăng nhập gần nhất khi off
        public string? Created_at { get; set; }
        public string? Updated_at { get; set; }
    }
}
