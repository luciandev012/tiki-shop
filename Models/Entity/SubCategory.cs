using MongoDB.Bson.Serialization.Attributes;

namespace tiki_shop.Models.Entity
{
    public class SubCategory
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("name")]
        public string? Name { get; set; }
        [BsonElement("image")]
        public string? Image { get; set; }
    }
}
