using MongoDB.Bson.Serialization.Attributes;

namespace SupportChat
{
    public class IntDTO
    {
        [BsonElement("value")]
        public int Value;
    }
}