using DataBase.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services.Account;
using TaskManagementAPI.Services.Profile;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ProfileService _profileService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProfileController(ApplicationDbContext db, ProfileService profileService, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _profileService = profileService;
            _contextAccessor = contextAccessor;
        }

        [HttpPost]
        public async Task<ActionResult> Profile([FromBody] GetUserId model)
        {
            try
            {
                var (success, message, data) = await _profileService.Profile(model.id);
                return Ok(new
                {
                    status = (int)success,
                    message = message,
                    data
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    status = 500,
                    message = "Đã có lỗi xảy ra ở server",
                    error = ex.Message
                });
            }
        }

        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDTO model)
        {
            try
            {
                var (success, message) = await _profileService.UpdateProfileService(model);

                return Ok(new
                {
                    status = (int)success,
                    message = message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    message = "Đã có lỗi xảy ra ở server",
                    error = ex.Message
                });
            }
        }

    }
}
