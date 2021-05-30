using System;
using MongoDB.Bson.Serialization.Attributes;

namespace SupportChat
{
    public class MessageDTO//Id?
    {
        [BsonElement("message")]
        public string Message;
        [BsonElement("userId")]
        public int UserId;
        [BsonElement("time")]
        public DateTime Time;
        [BsonElement("isMessageFromUser")]
        public bool IsMessageFromUser;
    }
}