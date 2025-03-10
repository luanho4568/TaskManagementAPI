using DataBase.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Utilities.Constants;
using TaskManagementAPI.Utilities.Cookies;
using TaskManagementAPI.Utilities.JwtAuthentication;
using Org.BouncyCastle.Asn1.Ocsp;
using DataBase.Models;
using TaskManagementAPI.Utilities.Enums;
using TaskManagementAPI.Utilities.Common;
using System.Text;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services.Account
{
    public class AccountService
    {
        private readonly ApplicationDbContext _db;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;

        public AccountService(ApplicationDbContext db, JwtService jwtService, IConfiguration configuration, IHttpContextAccessor context)
        {
            _db = db;
            _jwtService = jwtService;
            _configuration = configuration;
            _context = context;
        }
        public async Task<(LoginStatus, string, string)> LoginService([FromBody] LoginDTO model)
        {

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Type == TypeConstant.Local);
            if (user == null)
            {
                return (LoginStatus.InvalidCredentials, "Email không tồn tại!", string.Empty);
            }
            if (user.Lockout_EndTime.HasValue && user.Lockout_EndTime.Value > DateTime.UtcNow)
            {
                var remainingTime = (user.Lockout_EndTime.Value - DateTime.UtcNow).TotalSeconds;
                return (LoginStatus.LockedOut, $"Tài khoản đang bị khóa, vui lòng thử lại sau {remainingTime:F0} giây!", string.Empty);
            }

            var isPasswordValid = CommFunc.VerifyPassword(model.Password, user.HashPassword);
            if (!isPasswordValid)
            {
                user.Login_attempts = (user.Login_attempts ?? 0) + 1;

                if (user.Login_attempts >= 3)
                {
                    int attempts = user.Login_attempts.Value; // Chắc chắn rằng attempts không null
                    int lockoutDuration = Math.Min(5 * (attempts - 2), 15); // Tăng dần nhưng tối đa 30s
                    user.Lockout_EndTime = DateTime.UtcNow.AddSeconds(lockoutDuration);
                }
                _db.Entry(user).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return (LoginStatus.InvalidCredentials, "Email hoặc mật khẩu không đúng!", string.Empty);
            }

            var token = _jwtService.GenerateToken(user);
            //if (_jwtService.IsTokenExpired(token))
            //{
            //    return (LoginStatus.InvalidCredentials, "Token đã hết hạn, vui lòng đăng nhập lại!", string.Empty);
            //}

            user.Status = StatusConstant.Online;
            user.Last_login = DateTime.Now;
            user.Login_attempts = 0;
            user.Lockout_EndTime = null;
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return (LoginStatus.Success, "Đăng nhập thành công", token);
        }

        public async Task<(LogoutStatus, string)> LogoutService(string id)
        {
            var userLogin = await _db.Users.FindAsync(id);
            if (userLogin == null)
            {
                return (LogoutStatus.NotLoggedIn, "Không có người dùng");
            }
            userLogin.Status = StatusConstant.Offline;
            userLogin.Status_login = DateTime.Now;
            _db.Entry(userLogin).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return (LogoutStatus.Success, "Đăng xuất thành công");
        }

        public async Task<(RegisterStatus, string)> RegisterService([FromBody] Users model)
        {

            var email = await _db.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (email != null)
            {
                return (RegisterStatus.AlreadyExists, "Email đã tồn tại!");
            }
            var phone = await _db.Users.FirstOrDefaultAsync(x => x.Phone == model.Phone);
            if (phone != null)
            {
                return (RegisterStatus.AlreadyExists, "Số điện thoại đã tồn tại!");
            }

            var hashedPassword = CommFunc.HashPassword(model.Password);

            var newUser = new Users
            {
                Id = CommFunc.NewShortId(),
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                HashPassword = hashedPassword,
                Password = model.Password,
                Type = TypeConstant.Local,
                Role = RoleConstant.Member,
                Created_at = DateTime.Now,
                PersonalLink = CommFunc.GetNextAutoIncrementValue<Users>(_db, x => x.PersonalLink)
            };

            await _db.AddAsync(newUser);
            await _db.SaveChangesAsync();
            return (RegisterStatus.Success, "Đăng ký thành công!");
        }
        public IDictionary<string, string> DecodeToken(string token)
        {
            return _jwtService.DecodeToken(token);
        }

    }
}
