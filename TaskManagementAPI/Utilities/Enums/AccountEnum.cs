namespace TaskManagementAPI.Utilities.Enums
{
    /// <summary>
    /// Trạng thái đăng nhập của người dùng
    /// </summary>
    public enum LoginStatus
    {
        Success = 0,           // Đăng nhập thành công
        InvalidCredentials = -1, // Sai email hoặc mật khẩu
        LockedOut = -2          // Bị khóa tài khoản
    }

    /// <summary>
    /// Trạng thái kiểm tra đăng nhập
    /// </summary>
    public enum CheckLoginStatus
    {
        NotLoggedIn = -1,        // Chưa đăng nhập
        SessionExpired = -2,    // Đăng nhập hết hạn
        Active = 0              // Người dùng đang hoạt động
    }

    /// <summary>
    /// Trạng thái đăng xuất
    /// </summary>
    public enum LogoutStatus
    {
        NotLoggedIn = -1,        // Người dùng chưa đăng nhập
        Success = 0           // Đăng xuất thành công
    }

    /// <summary>
    /// Trạng thái đăng ký của người dùng
    /// </summary>
    public enum RegisterStatus
    {
        Success = 0,          // Đăng ký thành công
        AlreadyExists = -1,   // Đã tồn tại
        InvalidFormat = -2    // Sai định dạng (Regex không hợp lệ)
    }
}
