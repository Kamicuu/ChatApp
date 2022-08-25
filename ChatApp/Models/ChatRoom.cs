using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    internal class ChatRoom
    {
        public ChatRoom() 
        {
            ChatUsers = new List<ChatUser>();
        }
        internal string ChatRoomName { get; set; }
        internal Guid ChatRoomId { get; set; }
        internal IEnumerable<ChatUser> ChatUsers { get; }
        internal string CreatedBy { get; set; }
    }
}
