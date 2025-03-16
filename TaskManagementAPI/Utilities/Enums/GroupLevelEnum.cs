namespace TaskManagementAPI.Utilities.Enums
{
    [Flags]
    public enum GroupPermission
    {
        None = 0,             // Không có quyền
        ViewTask = 1 << 0,    // 0000001 = 1  -> Xem Task
        UpdateTask = 1 << 1,  // 0000010 = 2  -> Cập nhật Task
        CreateTask = 1 << 2,  // 0000100 = 4  -> Thêm Task mới
        EditTask = 1 << 3,    // 0001000 = 8  -> Chỉnh sửa Task
        DeleteTask = 1 << 4,  // 0010000 = 16 -> Xóa Task
        ManageMembers = 1 << 5, // 0100000 = 32 -> Quản lý thành viên (mời/kick)
        ManageLevels = 1 << 6,  // 1000000 = 64 -> Quản lý cấp bậc trong group
        FullControl = ViewTask | UpdateTask | CreateTask | EditTask | DeleteTask | ManageMembers | ManageLevels
        // Full quyền = 1 + 2 + 4 + 8 + 16 + 32 + 64 = 127
    }
}
