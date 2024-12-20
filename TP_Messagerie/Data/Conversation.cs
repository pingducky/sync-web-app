using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TP_Messagerie.Data
{
    public class Conversation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("participants")]
        public List<Participant> Participants { get; set; } = null!;

        [BsonElement("lastMessage")]
        public string LastMessage { get; set; } = null!;

        [BsonElement("lastUpdated")]
        public DateTime LastUpdated { get; set; }
    }
}
