
namespace ChatApp.Constants
{
    public static class Commands
    {
        public const string USER_JOINED_EXIST_CHAT = "UserJoinedToExistingChat";
        public const string USER_JOINED_CHAT = "UserJoinedChat";
        public const string USER_DISCONNECTED_CHAT = "UserDisconnectedFromChat";
        public const string USER_NOT_DISCONNECTED_CHAT = "UserNotDisconnectedFromChat";
        public const string USER_NOT_JOINED_TO_CHAT = "UserNotJoinedToChat";
        public const string CHAT_EXISTS = "ChatExists";
        public const string CHAT_CREATED = "ChatCreated";
        public const string UNKNOW_INTERNAL_ERROR = "UnknowInternalError";
        public const string MESSAGE_SEND = "MessageSend";
        public const string MESSAGE_NOT_SEND = "MessageNotSend";
        public const string USERS_UPDATE = "ChatUsersUpdate";
    }
}
