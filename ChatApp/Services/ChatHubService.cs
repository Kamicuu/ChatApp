using ChatApp.Constants;
using ChatApp.Hubs;
using ChatApp.Models;
using ChatApp.Models.DTOs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public class ChatHubService : IChatHubService
    {
        private readonly IHubContext<ChatHub> hubContext;

        //Example - database like data source
        private IEnumerable<ChatRoom> repository = Database.Instance.ChatRooms;

        public ChatHubService(IHubContext<ChatHub> context)
        {
            hubContext = context;
        }

        public async Task<DirectiveDTO> FindRoomForUserAsync(ChatUserDTO userDto, string connectionId)
        {
            try
            {
                var chatRoom = getChatRoomForUser(userDto.UserName);

                if (chatRoom != null)
                {
                    //ConnectionId is needed for sending message and validate if user use proper connection.
                    chatRoom.ChatUsers.First(user => user.UserName.Equals(userDto.UserName)).ConnectionID = connectionId;
                    await hubContext.Groups.AddToGroupAsync(connectionId, chatRoom.ChatRoomName);

                    return new DirectiveDTO(Commands.USER_JOINED_EXIST_CHAT, $"User reconnected to chat: {chatRoom.ChatRoomName}");
                }else if(!userDto.ChatRoomName.Equals(string.Empty))
                {
                    var chat = repository.FirstOrDefault(chatRoom => chatRoom.ChatRoomName.Equals(userDto.ChatRoomName));

                    if (chat != null) 
                    {
                        var chatUsers = (List<ChatUser>) chat.ChatUsers;

                        chatUsers.Add(new ChatUser(userDto.UserName, connectionId));
                        await hubContext.Groups.AddToGroupAsync(connectionId, chat.ChatRoomName);

                        return new DirectiveDTO(Commands.USER_JOINED_CHAT, "User joined to chat.");
                    }
                    return new DirectiveDTO(Commands.USER_NOT_JOINED_TO_CHAT, $"Chat with name: {userDto.ChatRoomName} - not exists!");
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
            return new DirectiveDTO(Commands.USER_NOT_JOINED_TO_CHAT, "User not joined to chat - not assigned to any chat or other error ocurs.");
        }

        public async Task<DirectiveDTO> SendMessageBySpecyficUser(ChatMessageDTO messageDto, string connectionId)
        {
            try
            {
                var chatRoom = getChatRoomForUser(messageDto.UserName);

                if (chatRoom != null)
                {
                    if(chatRoom.ChatUsers.First(user => user.UserName.Equals(messageDto.UserName)).ConnectionID.Equals(connectionId))
                    {
                        await hubContext.Clients.Groups(chatRoom.ChatRoomName).SendAsync(SignalMethods.RECIVE_MESSAGE, messageDto);

                        return new DirectiveDTO(Commands.MESSAGE_SEND, "Message send succesfully!");
                    }else return new DirectiveDTO(Commands.MESSAGE_NOT_SEND, "The message was not sent! ConnectionId not related with this chat.");
                }    
            }
            catch (InvalidOperationException ex)
            {
                //same users multiple times on different chats - teoretically impossible
            }
            catch (Exception ex)
            {
                //other errors
            }
            return new DirectiveDTO(Commands.MESSAGE_NOT_SEND, "The message was not sent! You are not connected to this chat.");
        }

        private ChatRoom getChatRoomForUser(string userName)
        {
            return repository.SingleOrDefault(
                    room => room.ChatUsers.FirstOrDefault(
                        user => user.UserName.Equals(userName)) != null);

        }
    }
}
