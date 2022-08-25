using ChatApp.Constants;
using ChatApp.Models;
using ChatApp.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private IChatHubService _chatService;
        public ChatHub(IChatHubService service)
        {
            _chatService = service;
        }

        public async Task ConnectToChat(ChatUserDTO chatUser)
        {
            var resp = await _chatService.FindRoomForUserAsync(chatUser, Context.ConnectionId);

            await Clients.Caller.SendAsync(SignalMethods.SEND_JOIN_MESSAGE, resp);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
