using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class ChatGroupMember
    {
        public string Id { get; set; }
        public string? ChatGroupId { get; set; }  // Thuộc nhóm chat nào
        public string? UserId { get; set; }  // Thành viên trong nhóm chat này
        public DateTime? JoinedAt { get; set; }
    }

}
