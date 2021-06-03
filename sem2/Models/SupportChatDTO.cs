using System.Collections.Generic;
using System.Linq;

namespace sem2.Models
{
    public class SupportChatDTO
    {
        public List<SupportChat.MessageDTO> Messages;
        public int UserId = -1;
    }
}