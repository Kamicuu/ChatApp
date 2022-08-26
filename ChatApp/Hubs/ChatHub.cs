using ChatApp.Constants;
using ChatApp.Models.DTOs;
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
        private IChatHubService chatService;
        public ChatHub(IChatHubService service)
        {
            chatService = service;
        }
        /// <summary>
        /// Connects user to chat if user is assgned to chat, esle sends information that user is not assigned to chat.
        /// </summary>
        /// <param name="chatUser"></param>
        /// <returns></returns>
        public async Task ConnectToChat(ChatUserDTO chatUser)
        {
            var resp = await chatService.FindRoomForUserAsync(chatUser, Context.ConnectionId);

            await Clients.Caller.SendAsync(SignalMethods.RECIVE_DIRECTIVE, resp);
        }
        public async Task SendMessage(ChatMessageDTO chatUser) 
        {
            var resp = await chatService.SendMessageBySpecyficUser(chatUser, Context.ConnectionId);

            await Clients.Caller.SendAsync(SignalMethods.RECIVE_DIRECTIVE, resp);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
