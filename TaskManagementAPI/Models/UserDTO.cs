using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class GetUserId
    {
        public string? id { get; set; }
    }

    public class UserDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public int? PersonalLink { get; set; } // Link cá nhân
        public string? Bio { get; set; } // giới thiệu
        public string? Status { get; set; } // online off
    }

    public class UpdateProfileDTO
    {
        public IFormFile? File { get; set; }
        public UserDTO? User { get; set; }
    }
}
