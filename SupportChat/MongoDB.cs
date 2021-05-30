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
        private readonly IMongoCollection<int> activeUsersCollection;
        
        
        //"mongodb+srv://mongodb:5nAA6B8RV6daPwtJ@mongodb.1yxjx.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"
        public MongoDB(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("supportMessages");
            messagesCollection = database.GetCollection<MessageDTO>("messages");
            activeUsersCollection = database.GetCollection<int>("messages");
        }
        
        public async Task<IOrderedEnumerable<MessageDTO>> GetMessages(int userId)
        {
            var messagesOfUser = await messagesCollection.FindAsync(x => x.UserId == userId);
            return messagesOfUser.ToList().OrderBy(x => x.Time);
        }

        public async Task AddMessage(int userId, string text, bool isMessageFromUser)
        {
            await messagesCollection.InsertOneAsync(new MessageDTO {UserId = userId, Message = text, Time = DateTime.Now, IsMessageFromUser = isMessageFromUser});
        }

        public async Task<List<int>> GetNotAnsweredUsers()
        {
            return (await activeUsersCollection.FindAsync(x => true)).ToList();
        }

        public async Task AddNotAnsweredUser(int id)
        {
            await activeUsersCollection.InsertOneAsync(id);
        }

        public async Task RemoveNotAnsweredUser(int id)
        {
            await activeUsersCollection.DeleteOneAsync(x => x == id);
        }

        public async Task ClearNotAnsweredUsers()
        {
            await activeUsersCollection.DeleteManyAsync(x => true);
        }
    }
}