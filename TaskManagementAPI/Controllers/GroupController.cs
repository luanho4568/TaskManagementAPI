using DataBase.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services.Group;

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
    }
}
