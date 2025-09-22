using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TransportApi.Models
{
    [BsonIgnoreExtraElements]
    public class Role
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        // Permission references by ObjectId string
        public List<string> PermissionIds { get; set; } = new List<string>();
    }
}