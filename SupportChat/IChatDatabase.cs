using System.Linq;
using System.Threading.Tasks;

namespace sem2
{
    public interface IChatDatabase
    {
        public Task<IOrderedEnumerable<MessageDTO>> GetMessages(int userId);

        public Task AddMessage(int userId, string text);
    }
}