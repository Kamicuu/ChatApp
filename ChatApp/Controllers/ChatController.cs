using ChatApp.Models;
using ChatApp.Models.DTOs;
using ChatApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

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

            return Ok(result);
        }

    }
}
