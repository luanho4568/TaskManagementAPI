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
}
