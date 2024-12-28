using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TP_Messagerie.Data
{
    public class MessageCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("sender")]
        public string Sender { get; set; } = null!;

        [BsonElement("receiver")]
        public string Receiver { get; set; } = null!;

        [BsonElement("content")]
        public string Content { get; set; } = null!;

        [BsonElement("sentAt")]
        public DateTime SentAt { get; set; }

        [BsonElement("readAt")]
        public DateTime? ReadAt { get; set; }
    }
}
