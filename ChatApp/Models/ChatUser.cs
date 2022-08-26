
namespace ChatApp.Models
{
    public class ChatUser
    {
        public ChatUser() { }
        public ChatUser(string userName, string connectionId)
        {
            UserName = userName;
            ConnectionID = connectionId;
        }
        public string UserName { get; set; }
        public string ConnectionID { get; set; }
    }
}
