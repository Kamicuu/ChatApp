using ChatApp.Constants;
using ChatApp.Models;
using ChatApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public class ChatControllerService : IChatControllerService
    {
        //Example - database like data source
        private List<ChatRoom> chatRooms = (List<ChatRoom>)Database.Instance.ChatRooms;

        public DirectiveDTO CreateNewChat(CreateNewChatDTO data, out HttpStatusCode httpStatusCode)
        {
            try
            {
                var chat = new ChatRoom();

                if (chatRooms.FirstOrDefault(chatRoom => chatRoom.ChatRoomName.Equals(data.ChatRoomName)) != null)
                {
                    httpStatusCode = HttpStatusCode.Conflict;
                    return new DirectiveDTO(Commands.CHAT_EXISTS, $"Chat with name {data.ChatRoomName} alredy exists!");
                }

                chat.ChatRoomName = data.ChatRoomName;
                chat.ChatRoomId = Guid.NewGuid();
                chat.CreatedBy = data.CreatedBy;

                chatRooms.Add(chat);

                httpStatusCode = HttpStatusCode.Created;
                return new DirectiveDTO(Commands.CHAT_CREATED, $"Chat with name {data.ChatRoomName} was created!");
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}
