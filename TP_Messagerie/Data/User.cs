using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace TP_Messagerie.Data
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("username")]
        public string Username { get; set; } = null!;

        [BsonElement("password")]
        public string Password { get; set; } = null!;

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("firstname")]
        public string Firstname { get; set; } = null!;
    }
}
