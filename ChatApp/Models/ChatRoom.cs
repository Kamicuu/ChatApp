using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class ChatRoom
    {
        public string ChatRoomName { get; set; }
        public Guid ChatRoomId { get; set; }
        public IEnumerable<ChatUser> ChatUsers { get; } = new List<ChatUser>();
        public string CreatedBy { get; set; }
    }
}
