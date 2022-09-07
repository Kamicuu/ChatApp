using ChatApp.Models;
using ChatApp.Models.DTOs;
using ChatApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace ChatApp.Controllers
{

    [Route("api")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class ChatController : ControllerBase
    {
        private IChatControllerService<HttpStatusCode> chatService;
        public ChatController(IChatControllerService<HttpStatusCode> service)
        {
            chatService = service;
        }

        [HttpPost("chat")]
        public IActionResult CreateNewChat(CreateNewChatDTO data)
        {
            var result = chatService.CreateNewChat(data, out HttpStatusCode statusCode);

            if (statusCode == HttpStatusCode.Conflict)
            {
                return Conflict(result);
            }
            else if(statusCode == HttpStatusCode.InternalServerError)
            {
                return StatusCode(500, result);
            }

            return Created(Request.Path, result);
        }

        [HttpGet("chats")]
        public IActionResult GetAllChats()
        {
            var result = chatService.GetAllChatsList(out HttpStatusCode statusCode);

            if (statusCode == HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            else if (statusCode == HttpStatusCode.InternalServerError)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }

    }
}
