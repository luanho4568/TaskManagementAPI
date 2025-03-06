using DataBase.Data;
using DataBase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Services.Account;
using TaskManagementAPI.Utilities.Constants;
using TaskManagementAPI.Utilities.Cookies;
using TaskManagementAPI.Utilities.Enums;
using TaskManagementAPI.Utilities.JwtAuthentication;
using TaskManagementAPI.Utilities.Sessions;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly AccountService _accountService;

        public AccountController(ApplicationDbContext db, JwtService jwtService, IConfiguration configuration, IHttpContextAccessor context, AccountService accountService)
        {
            _db = db;
            _jwtService = jwtService;
            _configuration = configuration;
            _context = context;
            _accountService = accountService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] Users model)
        {
            try
            {
                var (success, message) = await _accountService.LoginService(model);

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

        [HttpGet("CheckUserLogin")]
        public async Task<IActionResult> CheckUserLogin()
        {
            try
            {
                var (success, message) = await _accountService.CheckUserLoginService();

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

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var (success, message) = await _accountService.LogoutService();
                return Ok(new
                {
                    status = (int)success,
                    message = message
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

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] Users model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessage = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .FirstOrDefault() ?? "Dữ liệu không hợp lệ!";

                    return Ok(new
                    {
                        status = RegisterStatus.InvalidFormat,
                        message = errorMessage
                    });
                }
                var (success, message) = await _accountService.RegisterService(model);

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
