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

        public async Task<DirectiveDTO> DisconnectUserFromChat(ChatUserDTO userDto, string connectionId)
        {
            try
            {
                var chatRoom = getChatRoomForUser(ref repository, userDto.UserName);

                if (chatRoom != null)
                {
                    //validation
                    if (validateChatUserConnectionId(ref chatRoom, userDto.UserName, connectionId))
                    {
                        var chatUsers = (List<ChatUser>)chatRoom.ChatUsers;
                        chatUsers.RemoveAll(user => user.UserName == userDto.UserName);
                        await hubContext.Groups.RemoveFromGroupAsync(connectionId, chatRoom.ChatRoomName);

                        return new DirectiveDTO(Commands.USER_DISCONNECTED_CHAT, $"User {userDto.UserName} was disconnected from chat.");
                    }
                    else new DirectiveDTO(Commands.USER_NOT_DISCONNECTED_CHAT, $"User {userDto.UserName} was't disconnected. ConnectionId not related with this chat/user.");
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

            return new DirectiveDTO(Commands.USER_NOT_DISCONNECTED_CHAT, $"User {userDto.UserName} was't disconnected. User are not connected to any chat.");
        }

        public async Task<DirectiveDTO> FindRoomForUserAsync(ChatUserDTO userDto, string connectionId)
        {
            try
            {
                var chatRoom = getChatRoomForUser(ref repository, userDto.UserName);

                if (chatRoom != null)
                {
                    //ConnectionId is needed for sending message and validate if user use proper connection.
                    var chatUser = chatRoom.ChatUsers.First(user => user.UserName.Equals(userDto.UserName));

                    await hubContext.Groups.RemoveFromGroupAsync(chatUser.ConnectionID, chatRoom.ChatRoomName);
                    chatUser.ConnectionID = connectionId;
                    await hubContext.Groups.AddToGroupAsync(connectionId, chatRoom.ChatRoomName);

                    return new DirectiveDTO(Commands.USER_JOINED_EXIST_CHAT, chatRoom.ChatRoomName);
                }
                else if(!userDto.ChatRoomName.Equals(string.Empty))
                {
                    var chat = repository.FirstOrDefault(chatRoom => chatRoom.ChatRoomName.Equals(userDto.ChatRoomName));

                    if (chat != null) 
                    {
                        var chatUsers = (List<ChatUser>) chat.ChatUsers;

                        chatUsers.Add(new ChatUser(userDto.UserName, connectionId));
                        await hubContext.Groups.AddToGroupAsync(connectionId, chat.ChatRoomName);

                        return new DirectiveDTO(Commands.USER_JOINED_CHAT, chat.ChatRoomName);
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
                var chatRoom = getChatRoomForUser(ref repository, messageDto.UserName);

                if (chatRoom != null)
                {
                    if(validateChatUserConnectionId(ref chatRoom, messageDto.UserName, connectionId))
                    {
                        await hubContext.Clients.Groups(chatRoom.ChatRoomName).SendAsync(SignalMethods.RECIVE_MESSAGE, messageDto);

                        return new DirectiveDTO(Commands.MESSAGE_SEND, "Message send succesfully!");
                    }
                    else return new DirectiveDTO(Commands.MESSAGE_NOT_SEND, "The message was not sent! ConnectionId not related with this chat/user.");
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

        /// <summary>
        /// Returns ChatRoom with assigned user, if user not exists in list returns null, if user exists more than one time exeption is thrown.
        /// </summary>
        /// <param name="chatRooms"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static ChatRoom getChatRoomForUser(ref IEnumerable<ChatRoom> chatRooms, string userName)
        {
            return chatRooms.SingleOrDefault(
                    room => room.ChatUsers.FirstOrDefault(
                        user => user.UserName.Equals(userName)) != null);

        }

        /// <summary>
        /// Checks if in given ChatRoom is user with given connectionId, if in chatRoom is multiple users with same username exeption is thrown.
        /// </summary>
        /// <param name="chatRoom"></param>
        /// <param name="userName"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        private static bool validateChatUserConnectionId(ref ChatRoom chatRoom, string userName, string connectionId)
        {
            return chatRoom.ChatUsers.First(user => user.UserName.Equals(userName)).ConnectionID.Equals(connectionId);
        }
    }
}
