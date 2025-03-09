namespace TaskManagementAPI.Models
{
    public class LoginVM
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class LogoutVM
    {
        public string? id { get; set; }
    }
}
