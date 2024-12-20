﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace TP_Messagerie.Data
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }
    }
}
