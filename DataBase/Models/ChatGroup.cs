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
        public string? Name { get; set; }  // Tên nhóm chat (kênh chung sẽ có tên cố định)
        public string? CreatedBy { get; set; }
        public bool? IsDefault { get; set; }  // Có phải kênh mặc định của Group không?
        public DateTime? CreatedAt { get; set; }
    }

}
