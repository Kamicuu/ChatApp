
namespace ChatApp.Models.DTOs
{
    public class DirectiveDTO<T>
    {
        public DirectiveDTO() { }
        public DirectiveDTO(string command, T message)
        {
            Command = command;
            Message = message;
        }
        public string Command { get; set; }
        public T Message { get; set; }
    }
}
