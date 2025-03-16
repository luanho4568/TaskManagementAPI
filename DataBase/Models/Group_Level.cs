using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class Group_Level
    {
        public string Id { get; set; }        
        public int? GroupId { get; set; }    // Nhóm nào sử dụng level này
        public int? Level { get; set; }         // Cấp độ (1 -> thấp nhất, 999 -> cao nhất)
        public string? Name { get; set; }       // Tên level (ví dụ: "Thành viên", "Leader", "Manager", "Owner")
        public int? Permissions { get; set; } // Quyền hạn của level này (Enum)
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
