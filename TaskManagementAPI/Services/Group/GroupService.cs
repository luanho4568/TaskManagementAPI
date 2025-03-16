using DataBase.Data;
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
                MemberCount = 0,
                Settings = model.Settings ?? GroupSettingStatus.Private.ToString(),
                CreatedAt = DateTime.Now
            };
            var newGroupMember = new DataBase.Models.Group_Member
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
            await _db.AddRangeAsync(newGroup, newGroupMember); 
            await _db.SaveChangesAsync();
        }
    }
}
