using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TP_Messagerie.Data
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("senderId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SenderId { get; set; } = null!;

        [BsonElement("receiverId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReceiverId { get; set; } = null!;

        [BsonElement("content")]
        public string Content { get; set; } = null!;

        [BsonElement("sentAt")]
        public DateTime SentAt { get; set; }

        [BsonElement("readAt")]
        public DateTime? ReadAt { get; set; }
    }
}
