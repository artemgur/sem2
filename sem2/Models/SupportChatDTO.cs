using System.Linq;

namespace sem2.Models
{
    public class SupportChatDTO
    {
        public IOrderedEnumerable<SupportChat.MessageDTO> Messages;
        public int? UserId;
    }
}