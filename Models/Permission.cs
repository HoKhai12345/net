using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TransportApi.Models
{
    [BsonIgnoreExtraElements]
    public class Permission
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}