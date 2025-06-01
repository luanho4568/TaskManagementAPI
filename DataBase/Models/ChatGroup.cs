using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class ChatGroup
    {
        public string Id { get; set; } 
        public int? GroupId { get; set; }  // Nhóm cha của kênh chat này
        public string? Name { get; set; }  
        public string? Type { get; set; } // default và tự tạo
        public string? CreatedBy { get; set; }
        public bool? IsDefault { get; set; }  // Có phải kênh mặc định của Group không?
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; } // Trạng thái của kênh chat (ví dụ: "public", "Private", "Restricted")
        public string? Avatar { get; set; } // Hình đại diện của kênh chat
    }

}
