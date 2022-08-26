
namespace ChatApp.Models.DTOs
{
    public class DirectiveDTO
    {
        public DirectiveDTO() { }
        public DirectiveDTO(string command, string message)
        {
            Command = command;
            Message = message;
        }
        public string Command { get; set; }
        public string Message { get; set; }
    }
}
