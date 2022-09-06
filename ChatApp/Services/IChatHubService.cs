﻿using ChatApp.Models.DTOs;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface IChatHubService
    {
        /// <summary>
        /// Search exiting chats database and add connection to chat when user exists in database, when user is not assigned to any chat and sends proper chatName, then user is assigned to chat.
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public Task<DirectiveDTO> FindRoomForUserAsync(ChatUserDTO userDto, string connectionId);
        /// <summary>
        /// Send message to clients assigned to same chat as client with derivated connectionId.
        /// </summary>
        /// <param name="messageDto"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public Task<DirectiveDTO> SendMessageBySpecyficUser(ChatMessageDTO messageDto, string connectionId);
        /// <summary>
        /// Disconected selected user form chat
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public Task<DirectiveDTO> DisconnectUserFromChat(ChatUserDTO userDto, string connectionId);
    }
}