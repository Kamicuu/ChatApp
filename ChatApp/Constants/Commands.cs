using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Constants
{
    public static class Commands
    {
        public static readonly string USER_JOINED_EXIST_CHAT = "UserJoinedToExistingChat";
        public static readonly string USER_NOT_JOINED_TO_CHAT = "UserNotJoinedToChat";
        public static readonly string CHAT_EXISTS = "ChatExists";
        public static readonly string CHAT_CREATED = "ChatCreated";
        public static readonly string UNKNOW_INTERNAL_ERROR = "UnknowInternalError";
    }
}
