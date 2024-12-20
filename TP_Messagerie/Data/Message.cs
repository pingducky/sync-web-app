using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TP_Messagerie.Data
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string SenderId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string ReceiverId { get; set; } = null!;

        public string Content { get; set; } = null!;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime SentAt { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? ReadAt { get; set; }
    }
}
