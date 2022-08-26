using ChatApp.Models;
using ChatApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface IChatControllerService<T>
    {
        /// <summary>
        /// Method create new chat or returns info if chat exists
        /// </summary>
        /// <param name="data"></param>
        /// <param name="httpStatusCode">Conflict - when chat exsits, Created - when chat created, Internal Error - when other exeption occurs</param>
        /// <returns></returns>
        public DirectiveDTO CreateNewChat(CreateNewChatDTO data, out T statusCode);
        /// <summary>
        /// Returns all existing chats
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public List<ChatRoomDTO> GetAllChatsList(out T statusCode);
    }
}
