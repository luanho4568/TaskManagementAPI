using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class Group_Member
    {
        public string Id { get; set; }
        public int? GroupId { get; set; }
        public string? UserId { get; set; }
        public string? Role { get; set; }
        public int? Level { get; set; }
        public int? TaskCount { get; set; }
        public string? Status { get; set; }
        public DateTime Joined_at { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdateBy { get; set; }
    }
}
