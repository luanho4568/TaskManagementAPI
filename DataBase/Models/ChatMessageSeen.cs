using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class ChatMessageSeen
    {
        public string Id { get; set; } 
        public string? MessageId { get; set; }  // Tin nhắn nào
        public string? UserId { get; set; }  // Ai đã xem tin nhắn này
        public DateTime SeenAt { get; set; }  // Thời gian đã xem
    }

}
