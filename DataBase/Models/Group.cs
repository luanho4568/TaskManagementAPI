using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? ShortDescription { get; set; }

        public int? MemberCount { get; set; }
        public int? ProjectCount { get; set; }
        public string? OwnerId { get; set; } // id user -> người chủ tạo group
        public string? Settings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
