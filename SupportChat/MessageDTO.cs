using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SupportChat
{
    public class MessageDTO//Id?
    {
        [BsonId]
        public ObjectId Id { get; set; }
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