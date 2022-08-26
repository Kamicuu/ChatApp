using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models.DTOs
{
    public class ChatMessageDTO : ChatMessage
    {
        public string UserName { get; set; }
    }
}
