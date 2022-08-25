using ChatApp.Constants;
using ChatApp.Hubs;
using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public class ChatHubService : IChatHubService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        //Example - database like data source
        private IEnumerable<ChatRoom> chatRooms = Database.Instance.ChatRooms;

        public ChatHubService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<DirectiveDTO> FindRoomForUserAsync(ChatUserDTO userDto, string connectionId)
        {
            try
            {
                
                var chatRoom = chatRooms.SingleOrDefault(
                    room => room.ChatUsers.FirstOrDefault(
                        user => user.UserName.Equals(userDto.UserName)) != null);

                if (chatRoom != null)
                {
                    await _hubContext.Groups.AddToGroupAsync(connectionId, chatRoom.ChatRoomName);
                    return new DirectiveDTO(Commands.USER_JOINED_EXIST_CHAT, "User joined to existing group.");
                }
            }
            catch(InvalidOperationException ex)
            {
                //username is used by another user
            }
            catch (Exception ex)
            {
                //other errors
            }
            return new DirectiveDTO(Commands.USER_NOT_JOINED_TO_CHAT, "User not joined to chat - not assigned to any group.");
        }
    }
}
