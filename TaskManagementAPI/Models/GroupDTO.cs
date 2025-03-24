using DataBase.Models;

namespace TaskManagementAPI.Models
{
    public class GroupDTO
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? Settings { get; set; }
    }

    public class GroupModel
    {
        public Group? Group { get; set; }
        public Group_Member? Group_Member { get; set; }
        public Group_Level? Group_Level { get; set; }
        public ChatGroup? ChatGroup { get; set; }
        public ChatGroupMember? ChatGroupMember { get; set; }
        public ChatMessage? ChatMessage { get; set; }
        public ChatMessageSeen? ChatMessageSeen { get; set; }
    }
}
