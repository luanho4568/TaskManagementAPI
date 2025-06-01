using DataBase.Data;
using Google.Protobuf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Services.Group;
using TaskManagementAPI.Utilities.Enums;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ChatService _chatService;

        public ChatController(ApplicationDbContext db, ChatService chatService)
        {
            _db = db;
            _chatService = chatService;
        }

        [HttpGet("JoinChat")]
        public async Task<IActionResult> JoinChat(int groupId)
        {
            try
            {

                var (status, mess) = await _chatService.JoinChatAsync(groupId);

                return Ok(new
                {
                    status = status,
                    message = mess
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

        [HttpGet("GetGroupChat")]
        public async Task<IActionResult> GetGroupChat(int groupId)
        {
            try
            {
                var (status, message, data) = await _chatService.GetGroupChatAsync(groupId);

                return Ok(new
                {
                    status = status,
                    message = message,
                    data = data
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
