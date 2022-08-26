using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models.DTOs
{
    public class ChatRoomDTO : ChatRoom
    {
        public int NumberOfActiveUsers { get; set; }
    }
}
