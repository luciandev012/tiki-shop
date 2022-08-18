using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace tiki_shop.Models.Entity
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("fullName")]
        public string? Fullname { get; set; }
        [BsonElement("phoneNumber")]
        public string? PhoneNumber { get; set; }
        [BsonElement("password")]
        public string? Password { get; set; }
        [BsonElement("balance")]
        public float Balance { get; set; } = 0;
        [BsonElement("email")]
        public string? Email { get; set; }
        [BsonElement("address")]
        public string? Address { get; set; }
        [BsonElement("status")]
        public bool Status { get; set; } = true;
        [BsonElement("role")]
        public string? Role { get; set; }
        //[BsonRepresentation(BsonType.Array)]
        //public List<Recharge>? Recharges { get; set; }
    }
}
