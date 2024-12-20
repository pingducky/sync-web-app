using MongoDB.Bson.Serialization.Attributes;

namespace TP_Messagerie.Data
{
    public class Participant
    {
        [BsonElement("userId")]
        public string UserId { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("firstname")]
        public string Firstname { get; set; } = null!;

        [BsonElement("username")]
        public string Username { get; set; } = null!;
    }
}
