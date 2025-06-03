using DataBase.Data;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagementAPI.Models;
using TaskManagementAPI.Utilities.Common;
using TaskManagementAPI.Utilities.Enums;

namespace TaskManagementAPI.Services.Group
{
    public class GroupService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GroupService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateGroupService(GroupDTO model)
        {

            var newGroup = new DataBase.Models.Group
            {
                Id = CommFunc.GetNextAutoIncrementValue<DataBase.Models.Group>(_db, x => x.Id),
                Name = model.Name,
                ShortDescription = model.ShortDescription,
                OwnerId = model.UserId,
                ProjectCount = 0,
                MemberCount = 1,
                Settings = model.Settings ?? GroupSettingStatus.Private.ToString(),
                CreatedAt = DateTime.Now
            };
            var newGroupMember = new Group_Member
            {
                Id = CommFunc.NewShortId(),
                GroupId = newGroup.Id,
                UserId = model.UserId,
                Role = RoleStatus.Owner.ToString(),
                Level = (int)LevelStatus.Owner,
                TaskCount = 0,
                Status = Status.Active.ToString(),
                Joined_at = DateTime.Now
            };
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            var newChatGroup = new ChatGroup
            {
                Id = CommFunc.NewShortId(),
                GroupId = newGroup.Id,
                Name = "Kênh chung",
                Type = ChatGroupType.Default.ToString(),
                IsDefault = true,
                CreatedBy = user.Name,
                CreatedAt = DateTime.Now
            };
            var newChatGroupMember = new ChatGroupMember
            {
                Id = CommFunc.NewShortId(),
                ChatGroupId = newChatGroup.Id,
                UserId = model.UserId,
                JoinedAt = DateTime.Now
            };
            await _db.AddRangeAsync(newGroup, newGroupMember, newChatGroup, newChatGroupMember);
            await _db.SaveChangesAsync();
        }

        public async Task<(int, string, List<GroupMemberDTO>)> GellAllMemberService(int groupId)
        {
            var members = from gm in _db.Group_Member
                          join u in _db.Users on gm.UserId equals u.Id
                          where gm.GroupId == groupId
                          select new GroupMemberDTO
                          {
                              GroupMembers = gm,
                              UserName = u.Name,
                              Avatar = u.Avatar
                          };
            var memberList = await members.ToListAsync();
            if (memberList.Count == 0)
            {
                return ((int)StatusEnum.Failed, "Không có thành viên nào trong nhóm này!", null!);
            }
            return ((int)StatusEnum.Success, "Lấy danh sách thành viên thành công!", memberList);
        }

        public async Task<(int, string)> JoinGroupService(string token)
        {
            var group = await _db.Group.FirstOrDefaultAsync(x => x.InviteLinkToken == token);
            if (group == null)
            {
                return ((int)StatusEnum.Failed, "Nhóm này không tồn tại");

            }
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingMember = await _db.Group_Member
                .FirstOrDefaultAsync(x => x.GroupId == group.Id && x.UserId == userId);
            if (existingMember != null)
            {
                return ((int)StatusEnum.Failed, "Bạn đã là thành viên của nhóm này!");
            }
            var newMember = new Group_Member
            {
                Id = Guid.NewGuid().ToString(),
                GroupId = group.Id,
                UserId = userId,
                Role = RoleStatus.Member.ToString(),
                Level = (int)LevelStatus.Member,
                TaskCount = 0,
                Status = Status.Pending.ToString(),
                Joined_at = DateTime.Now
            };
            await _db.Group_Member.AddAsync(newMember);
            group.MemberCount++;
            _db.Entry(group).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return ((int)StatusEnum.Success, "Gửi yêu cầu tham giá nhóm thành công!");
        }

        public async Task<(int, string, DataBase.Models.Group)> GetGroupService(int groupId)
        {
            var group = await _db.Group.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null)
            {
                return ((int)StatusEnum.Failed, "Nhóm này không tồn tại!", null!);
            }
            return ((int)StatusEnum.Success, "Lấy thông tin nhóm thành công!", group);
        }
    }
}
