using DataBase.Data;
using DataBase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Reflection.Metadata;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services.Group;
using TaskManagementAPI.Utilities.Enums;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly GroupService _groupService;

        public GroupController(ApplicationDbContext db, GroupService groupService)
        {
            _db = db;
            _groupService = groupService;
        }
        [HttpGet("GetAllGroup")]
        public async Task<IActionResult> GetAllGroup(string userId)
        {
            try
            {
                var query = (from g in _db.Group
                             join gm in _db.Group_Member
                             on g.Id equals gm.GroupId
                             where g.OwnerId == userId
                             select new
                             {
                                 g.Id,
                                 g.Name,
                                 g.ShortDescription,
                                 gm.Role,
                                 g.MemberCount,
                                 g.ProjectCount

                             }).ToList();
                return Ok(new
                {
                    status = 0,
                    data = query
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


        [HttpPost("CreateGroup")]
        public async Task<IActionResult> CreateGroup([FromBody] GroupDTO model)
        {
            try
            {
                await _groupService.CreateGroupService(model);
                return Ok(new
                {
                    status = 0,
                    message = "Tạo group thành công!"
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


        [HttpGet("JoinGroup")]
        public async Task<IActionResult> JoinGroup(int groupId)
        {
            var group = await _db.Group.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null)
            {
                return Ok(new
                {
                    status = -1,
                    message = "Nhóm này không tồn tại!"
                });
            }
            return Ok(new
            {
                status = 0,
                message = "Tham gia nhóm thành công!"
            });
        }

        [HttpDelete("DeleteGroup")]
        public async Task<IActionResult> DeleteGroup(int groupId, string userId)
        {
            try
            {
                var groupData = await (from g in _db.Group
                                       join gm in _db.Group_Member 
                                       on g.Id equals gm.GroupId into groupMembers
                                       from gm in groupMembers.DefaultIfEmpty()

                                       join gl in _db.Group_Level 
                                       on g.Id equals gl.GroupId into groupLevels
                                       from gl in groupLevels.DefaultIfEmpty()

                                       join cg in _db.ChatGroup 
                                       on g.Id equals cg.GroupId into chatGroups
                                       from cg in chatGroups.DefaultIfEmpty()

                                       join cgm in _db.ChatGroupMember 
                                       on cg.Id equals cgm.ChatGroupId into chatGroupMembers
                                       from cgm in chatGroupMembers.DefaultIfEmpty()

                                       join cm in _db.ChatMessage 
                                       on cg.Id equals cm.ChatGroupId into chatMessages
                                       from cm in chatMessages.DefaultIfEmpty()

                                       join cms in _db.ChatMessageSeen 
                                       on cm.Id equals cms.MessageId into chatMessageSeen
                                       from cms in chatMessageSeen.DefaultIfEmpty()

                                       where g.Id == groupId
                                       select new GroupModel
                                       {
                                           Group = g,
                                           Group_Member = gm,
                                           Group_Level = gl,
                                           ChatGroup = cg,
                                           ChatGroupMember = cgm,
                                           ChatMessage = cm,
                                           ChatMessageSeen = cms
                                       }).ToListAsync();

                if (!groupData.Any()) 
                {
                    return Ok(new { status = -1, message = "Nhóm này không tồn tại!" });
                }

                var groupMember = groupData.FirstOrDefault(x => x.Group_Member?.UserId == userId)?.Group_Member;
                if (groupMember == null || groupMember.Role != RoleStatus.Owner.ToString())
                {
                    return Ok(new { status = -2, message = "Bạn không có quyền xoá nhóm này!" });
                }

                _db.ChatMessageSeen.RemoveRange(groupData.Where(x => x.ChatMessageSeen != null).Select(x => x.ChatMessageSeen));
                _db.ChatMessage.RemoveRange(groupData.Where(x => x.ChatMessage != null).Select(x => x.ChatMessage));
                _db.ChatGroupMember.RemoveRange(groupData.Where(x => x.ChatGroupMember != null).Select(x => x.ChatGroupMember));
                _db.ChatGroup.RemoveRange(groupData.Where(x => x.ChatGroup != null).Select(x => x.ChatGroup));
                _db.Group_Level.RemoveRange(groupData.Where(x => x.Group_Level != null).Select(x => x.Group_Level));
                _db.Group_Member.RemoveRange(groupData.Where(x => x.Group_Member != null).Select(x => x.Group_Member));
                _db.Group.RemoveRange(groupData.Where(x => x.Group != null).Select(x => x.Group));

                await _db.SaveChangesAsync();
                return Ok(new { status = 0, message = "Xoá nhóm thành công!" });
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
