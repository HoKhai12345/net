using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Driver
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string VehicleId { get; set; }
}