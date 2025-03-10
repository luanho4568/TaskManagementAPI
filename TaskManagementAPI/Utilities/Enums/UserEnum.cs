namespace TaskManagementAPI.Utilities.Enums
{
    /// <summary>
    /// Trạng thái lấy ra user theo id
    /// </summary>
    public enum UserStatus
    {
        Success = 0,          // User tồn tại
        Failed = -1          // User không tồn tại
    }

    /// <summary>
    /// Trạng thái cập nhật user
    /// </summary>
    public enum UpdateUserStatus
    {
        Success = 0,          // Thành công
        Failed = -1          // Thất bại
    }
}
