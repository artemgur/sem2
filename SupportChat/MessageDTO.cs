using System;
using MongoDB.Bson.Serialization.Attributes;

namespace sem2
{
    public class MessageDTO//Id?
    {
        [BsonElement("message")]
        public string Message;
        [BsonElement("userId")]
        public int UserId;
        [BsonElement("time")]
        public DateTime Time;
    }
}