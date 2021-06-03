using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace SupportChat
{
    public class MongoDB: IChatDatabase
    {
        private readonly IMongoCollection<MessageDTO> messagesCollection;
        
        
        //"mongodb+srv://mongodb:5nAA6B8RV6daPwtJ@mongodb.1yxjx.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"
        public MongoDB()
        {
            var connectionString =
                "mongodb+srv://mongodb:5nAA6B8RV6daPwtJ@mongodb.1yxjx.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";//TODO 
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("supportMessages");
            messagesCollection = database.GetCollection<MessageDTO>("messages");
        }
        
        public async Task<List<MessageDTO>> GetMessages(int userId)
        {
            var messagesOfUser = await messagesCollection.FindAsync(x => x.UserId == userId);
            return messagesOfUser.ToList().OrderBy(x => x.Time).ToList();
        }

        public async Task AddMessage(int userId, string text, bool isMessageFromUser)
        {
            await messagesCollection.InsertOneAsync(new MessageDTO {UserId = userId, Message = text, Time = DateTime.Now, IsMessageFromUser = isMessageFromUser});
        }

        public async Task RemoveAllUserMessages(int userId)
        {
            await messagesCollection.DeleteManyAsync(x => x.UserId == userId);
        }
        
    }
}