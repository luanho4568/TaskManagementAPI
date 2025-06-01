using DataBase.Data;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using TaskManagementAPI.Models;
using TaskManagementAPI.Utilities.Enums;

namespace TaskManagementAPI.Services.Group
{
    public class ChatService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _contextAccessor;

        public ChatService(ApplicationDbContext db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }

        public async Task<(int, string)> JoinChatAsync(int groupId)
        {
            var group = await _db.Group.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null)
            {
                return ((int)StatusEnum.Failed, "Kênh chát này không tồn tại!");
            }
            return ((int)StatusEnum.Success, "Tham gia thành công!");
        }

        public async Task<(int, string, ChatGroupDTO)> GetGroupChatAsync(int groupId)
        {
            var group = await _db.Group.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null)
            {
                return ((int)StatusEnum.Failed, "Kênh chát này không tồn tại!", null);
            }
            var host = $"{_contextAccessor?.HttpContext?.Request.Scheme}://{_contextAccessor?.HttpContext?.Request.Host}";

            var members = await (from gm in _db.Group_Member
                                 join u in _db.Users on gm.UserId equals u.Id into joined
                                 from user in joined.DefaultIfEmpty()
                                 where gm.GroupId == groupId
                                 select new GroupMemberDTO
                                 {
                                     GroupMembers = gm,
                                     UserName = user.Name,
                                     Avatar = string.IsNullOrEmpty(user.Avatar) ? null : $"{host}/{user.Avatar}"
                                 }).ToListAsync();


            var chats = await _db.ChatGroup
                .Where(c => c.GroupId == groupId)
                .ToListAsync();

            var dto = new ChatGroupDTO
            {
                groupId = group.Id,
                GroupMembers = members,
                ChatGroup = chats
            };

            return ((int)StatusEnum.Success, "Lấy thông tin kênh chát thành công!", dto);
        }

    }
}
