using ChatApp.Constants;
using ChatApp.Models.DTOs;
using ChatApp.Services;
using Microsoft.AspNetCore.SignalR;
using System;
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
        /// Connects user to chat
        /// </summary>
        /// <param name="chatUser"></param>
        /// <returns></returns>
        public async Task ConnectToChat(ChatUserDTO chatUser)
        {
            var resp = await chatService.FindRoomForUserAsync(chatUser, Context.ConnectionId);
            await Clients.Caller.SendAsync(SignalMethods.RECIVE_DIRECTIVE, resp);
        }

        /// <summary>
        /// Sends standard message to chat
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        public async Task SendMessage(ChatMessageDTO chatMessage) 
        {
            var resp = await chatService.SendMessageBySpecyficUser(chatMessage, Context.ConnectionId);
            await Clients.Caller.SendAsync(SignalMethods.RECIVE_DIRECTIVE, resp);
        }

        /// <summary>
        /// Disconnects user from chat
        /// </summary>
        /// <param name="chatUser"></param>
        /// <returns></returns>
        public async Task DisconnectFromChat(ChatUserDTO chatUser)
        {
            var resp = await chatService.DisconnectUserFromChat(chatUser, Context.ConnectionId);
            await Clients.Caller.SendAsync(SignalMethods.RECIVE_DIRECTIVE, resp);
        }

        //to do - implementation of real user status
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
