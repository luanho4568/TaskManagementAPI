using System.Text.Json.Serialization;

namespace TaskManagementAPI.Utilities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleStatus
    {
        Member,  // Thành viên thường
        Manager, // Người quản lý nhóm
        Owner    // Người tạo nhóm
    }

    public enum Status
    {

        Active,   // Đang hoạt động
        Pending,  // Chờ duyệt vào nhóm
        Banned,   // Bị cấm
    }

    public enum LevelStatus
    {

        Member = 1,  // Cấp độ thấp nhất - Thành viên bình thường
        Owner = 999  // Cấp độ cao nhất - Chủ nhóm
    }
}
