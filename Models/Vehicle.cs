using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TransportApi.Models
{
    public class Vehicle
    {
        [BsonId] // Đánh dấu làm khóa chính trong Mongo
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;

        [BsonElement("plateNumber")]
        public string PlateNumber { get; set; } = null!;

        [BsonElement("type")]
        public string Type { get; set; } = null!;
    }
}
