using System.Text.Json.Serialization;

namespace TaskManagementAPI.Utilities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GroupSettingStatus
    {
        Public,  // Nhóm công khai
        Private, // Nhóm riêng tư
        Restricted // Nhóm hạn chế (chỉ tham gia khi được mời)
    }

    public enum DeleteGroupStatus
    {
        Deleted = 0,  // Nhóm đã bị xóa
        NotDeleted // Nhóm chưa bị xóa
    }
}
