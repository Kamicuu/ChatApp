using AutoMapper;
using ChatApp.Constants;
using ChatApp.Models;
using ChatApp.Models.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ChatApp.Services
{
    public class ChatControllerService : IChatControllerService<HttpStatusCode>
    {
        //Example - database like data source
        private List<ChatRoom> repository = (List<ChatRoom>)Database.Instance.ChatRooms;
        private readonly IMapper mapper;
        private readonly ILogger<ChatControllerService> logger;

        public ChatControllerService(IMapper mapper, ILogger<ChatControllerService> logger)
        {
            this.mapper = mapper;
            this.logger = logger;
        }

        public DirectiveDTO<string> CreateNewChat(CreateNewChatDTO data, out HttpStatusCode httpStatusCode)
        {
            try
            {
                var chat = new ChatRoom();

                if (repository.FirstOrDefault(chatRoom => chatRoom.ChatRoomName.Equals(data.ChatRoomName)) != null)
                {
                    httpStatusCode = HttpStatusCode.Conflict;

                    return new DirectiveDTO<string>(Commands.CHAT_EXISTS, $"Chat with name {data.ChatRoomName} alredy exists!");
                }

                chat.ChatRoomName = data.ChatRoomName;
                chat.ChatRoomId = Guid.NewGuid();
                chat.CreatedBy = data.CreatedBy;

                repository.Add(chat);

                httpStatusCode = HttpStatusCode.Created;

                return new DirectiveDTO<string>(Commands.CHAT_CREATED, $"Chat with name {data.ChatRoomName} was created!");
            }
            catch (Exception ex)
            {
                httpStatusCode = HttpStatusCode.InternalServerError;
                logger.LogError("Unexpected error occurs!", ex);

                return new DirectiveDTO<string>(Commands.UNKNOW_INTERNAL_ERROR, $"Unknow error!");
            }
            
        }

        public List<ChatRoomDTO> GetAllChatsList(out HttpStatusCode statusCode)
        {
            try
            {
                if (repository.Count != 0)
                    statusCode = HttpStatusCode.OK;
                else statusCode = HttpStatusCode.NotFound;

                var resultList = new List<ChatRoomDTO>();

                foreach (var ele in repository)
                {
                    var result = mapper.Map<ChatRoom,ChatRoomDTO>(ele);
                    result.NumberOfActiveUsers = result.ChatUsers.Count();
                    resultList.Add(result);
                }
            
                return resultList;
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.InternalServerError;
                logger.LogError("Unexpected error occurs!", ex);

                return new List<ChatRoomDTO>();
            }
        }
    }
}
