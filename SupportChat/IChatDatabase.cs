using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportChat
{
    public interface IChatDatabase
    {
        public Task<IOrderedEnumerable<MessageDTO>> GetMessages(int userId);

        public Task AddMessage(int userId, string text, bool isMessageFromUser);

        public Task<List<int>> GetActiveUsers();
        
        public Task AddActiveUser(int id);

        public Task RemoveActiveUser(int id);
        
        public Task ClearActiveUsers();
    }
}