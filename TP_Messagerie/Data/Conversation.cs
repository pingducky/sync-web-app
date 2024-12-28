using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TP_Messagerie.Data
{
    public class Conversation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("participants")]
        public List<string> Participants { get; set; } = null!;
            
        [BsonElement("lastMessage")]
        public string LastMessage { get; set; } = null!;

        [BsonElement("lastUserMessage")]
        public string LastUserMessage { get; set; } = null!;

        [BsonElement("lastUpdated")]
        public DateTime LastUpdated { get; set; }
    }
}
