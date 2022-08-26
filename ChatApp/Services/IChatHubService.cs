using ChatApp.Models.DTOs;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface IChatHubService
    {
        /// <summary>
        /// Search exiting chats database and add connection to chat when user exists in database. Returns false when user is not assigned to any chat in db.
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public Task<DirectiveDTO> FindRoomForUserAsync(ChatUserDTO userDto, string connectionId);
        public Task<DirectiveDTO> SendMessageBySpecyficUser(ChatMessageDTO messageDto, string connectionId);
    }
}