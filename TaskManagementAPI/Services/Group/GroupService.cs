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
                InviteLinkToken = CommFunc.GenerateGroupId(_db),
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
                Status = MemberStatus.Active.ToString(),
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

        public async Task<(int, string, List<GroupMemberDTO>)> GellAllMemberService(int groupId, string? statusMember)
        {
            var members = from gm in _db.Group_Member
                          join u in _db.Users on gm.UserId equals u.Id
                          where gm.GroupId == groupId
                          select new GroupMemberDTO
                          {
                              GroupMembers = gm,
                              UserName = u.Name!,
                              Avatar = u.Avatar ?? "uploads/avatars/default-avatar.jpg"
                          };
            if (!string.IsNullOrEmpty(statusMember))
            {
                members = members.Where(x => x.GroupMembers.Status.ToLower() == statusMember.ToLower());
            }
            var memberList = await members.ToListAsync();
            if (memberList.Count == 0)
            {
                return ((int)StatusEnum.Failed, "Không có thành viên nào trong nhóm này!", null!);
            }
            return ((int)StatusEnum.Success, "Lấy danh sách thành viên thành công!", memberList);
        }

        public async Task<(int, string)> JoinGroupService(JoinGroupDTO model)
        {
            var group = await _db.Group.FirstOrDefaultAsync(x => x.InviteLinkToken == model.token);
            if (group == null)
            {
                return ((int)StatusEnum.Failed, "Nhóm này không tồn tại");

            }
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingMember = await _db.Group_Member
                .FirstOrDefaultAsync(x => x.GroupId == group.Id && x.UserId == userId);
            if (existingMember != null)
            {
                if (existingMember.Status == MemberStatus.Active.ToString())
                {
                    return ((int)StatusEnum.Warning, "Bạn đã là thành viên của nhóm này!");
                }
                if (existingMember.Status == MemberStatus.Pending.ToString())
                {
                    return ((int)StatusEnum.Warning, "Bạn đã gửi yêu cầu tham gia nhóm này, vui lòng đợi phê duyệt!");
                }
                if (existingMember.Status == MemberStatus.Rejected.ToString())
                {
                    return ((int)StatusEnum.Warning, "Bạn đã bị từ chối tham gia nhóm này, vui lòng liên hệ quản trị viên để biết thêm chi tiết!");
                }
                return ((int)StatusEnum.Warning, "Bạn đã là thành viên của nhóm này với trạng thái khác!");
            }
            var newMember = new Group_Member
            {
                Id = Guid.NewGuid().ToString(),
                GroupId = group.Id,
                UserId = userId,
                Role = RoleStatus.Member.ToString(),
                Level = (int)LevelStatus.Member,
                TaskCount = 0,
                Status = MemberStatus.Pending.ToString(),
                Joined_at = DateTime.Now,
                Description = model.description ?? string.Empty
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

        public async Task<(int, string)> ManageMemberStatusService(ManageMemberGroupStatusDTO model)
        {
            var member = await _db.Group_Member
                .FirstOrDefaultAsync(x => x.GroupId == model.GroupId && x.UserId == model.UserId);
            if (member == null)
            {
                return ((int)StatusEnum.Failed, "Thành viên này không tồn tại trong nhóm!");
            }
            var message = string.Empty;
            switch (model.Status)
            {
                case var status when status == MemberStatus.Active.ToString():
                    member.Status = MemberStatus.Active.ToString();
                    message = "Thành viên đã được duyệt thành công!";
                    break;

                case var status when status == MemberStatus.Banned.ToString():
                    member.Status = MemberStatus.Banned.ToString();
                    message = "Thành viên đã bị cấm thành công!";
                    break;

                case var status when status == MemberStatus.Rejected.ToString():
                    member.Status = MemberStatus.Rejected.ToString();
                    message = "Thành viên đã bị từ chối tham gia nhóm thành công!";
                    break;

                default:
                    return ((int)StatusEnum.Failed, "Trạng thái thành viên không hợp lệ!");
                    break;
            }
            _db.Entry(member).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return ((int)StatusEnum.Success, message);
        }
    }
}
