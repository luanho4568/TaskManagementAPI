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

    public record ChatGroupDTO
    {
        public int? groupId { get; set; }  // Nhóm cha của kênh chat này
        public List<ChatGroup>? ChatGroup { get; set; }  // Danh sách kênh chat
        public List<GroupMemberDTO>? GroupMembers { get; set; }  // Danh sách thành viên
    }

    public record GroupMemberDTO
    {
        public Group_Member GroupMembers { get; set; }  // Thông tin thành viên trong nhóm
        public string UserName { get; set; }  // Tên người dùng
        public string Avatar { get; set; }  // Hình đại diện của người dùng
    }
}