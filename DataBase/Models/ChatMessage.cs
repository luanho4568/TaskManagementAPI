using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class ChatMessage
    {
        public string Id { get; set; }
        public string? ChatGroupId { get; set; }  // Nhóm chat nào
        public string? SenderId { get; set; }  // Ai gửi tin nhắn
        public string? Content { get; set; }  // Nội dung tin nhắn
        public bool? IsEdited { get; set; }  // Tin nhắn đã chỉnh sửa chưa?
        public bool? IsDeleted { get; set; }  // Tin nhắn có bị xoá không?
        public bool? IsRevoked { get; set; }  // Tin nhắn bị thu hồi không?
        public DateTime? SentAt { get; set; } // Thời gian gửi tin nhắn
        public DateTime? EditedAt { get; set; }  // Thời gian chỉnh sửa
    }

}
