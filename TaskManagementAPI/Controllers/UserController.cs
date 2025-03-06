using DataBase.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetUser")]
        public async Task<ActionResult> GetUser()
        {
            try
            {
                var users = await _db.Users.ToListAsync();
                if (users == null || users.Count == 0) return Ok(new
                {
                    status = false,
                    message = "Chưa có người dùng nào!"
                });
                return Ok(new
                {
                    status = true,
                    message = "Lấy danh sách người dùng thành công",
                    data = users
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    status = false,
                    message = "Đã có lỗi xảy ra ở server",
                    error = ex.Message
                });
            }
        }
    }
}
