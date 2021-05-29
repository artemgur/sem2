using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace sem2
{
    public class MongoDB: IChatDatabase
    {
        private readonly IMongoCollection<MessageDTO> collection;
        
        //"mongodb+srv://mongodb:5nAA6B8RV6daPwtJ@mongodb.1yxjx.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"
        public MongoDB(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("supportMessages");
            collection = database.GetCollection<MessageDTO>("messages");
        }
        
        public async Task<IOrderedEnumerable<MessageDTO>> GetMessages(int userId)
        {
            var messagesOfUser = await collection.FindAsync(x => x.UserId == userId);
            return messagesOfUser.ToList().OrderBy(x => x.Time);
        }

        public async Task AddMessage(int userId, string text)
        {
            await collection.InsertOneAsync(new MessageDTO {UserId = userId, Message = text, Time = DateTime.Now});
        }
    }
}