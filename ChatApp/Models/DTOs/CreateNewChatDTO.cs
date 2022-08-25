using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models.DTOs
{
    public class CreateNewChatDTO
    {
        public string ChatRoomName { get; set; }
        public string CreatedBy { get; set; }
    }
}
