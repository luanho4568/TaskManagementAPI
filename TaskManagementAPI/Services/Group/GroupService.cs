using DataBase.Data;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;
using TaskManagementAPI.Utilities.Common;
using TaskManagementAPI.Utilities.Enums;

namespace TaskManagementAPI.Services.Group
{
    public class GroupService
    {
        private readonly ApplicationDbContext _db;
        public GroupService(ApplicationDbContext db)
        {
            _db = db;
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
            await _db.AddRangeAsync(newGroup, newGroupMember,newChatGroup,newChatGroupMember); 
            await _db.SaveChangesAsync();
        }
    }
}
