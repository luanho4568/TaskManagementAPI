using DataBase.Data;
using DataBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TaskManagementAPI.Models;
using TaskManagementAPI.Utilities.Common;
using TaskManagementAPI.Utilities.Enums;

namespace TaskManagementAPI.Services.Profile
{
    public class ProfileService
    {
        private readonly ApplicationDbContext _db;
        private readonly string _uploadFolder = "wwwroot/uploads/avatars";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string host;

        public ProfileService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            host = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}";
        }
        public async Task<(UserStatus, string, UserDTO)> Profile(string id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return (UserStatus.Failed, "Không tìm thấy người dùng", null);
            var userVM = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Avatar = $"{host}/{user.Avatar}",
                PersonalLink = user.PersonalLink,
                Bio = user.Bio,
                Status = user.Status
            };
            return (UserStatus.Success, "Có người dùng", userVM);
        }

        public async Task<(UpdateUserStatus, string)> UpdateProfileService(UpdateProfileDTO model)
        {
            var user = await _db.Users.FindAsync(model.User.Id);
            if (user == null) return (UpdateUserStatus.Failed, "Người dùng không tồn tại");
            if (model.File != null)
            {
                string uploadPath = "wwwroot/uploads/avatars";
                string newAvatar = await CommFunc.UploadFile(model.File, uploadPath);
                user.Avatar = $"/uploads/avatars/{newAvatar}";
            }
            user.Name = model.User.Name;
            user.Email = model.User.Email;
            user.Phone = model.User.Phone;
            user.Address = model.User.Address;
            user.Bio = model.User.Bio;

            await _db.SaveChangesAsync();
            return (UpdateUserStatus.Success, "Cập nhật thành công");
        }
    }
}
