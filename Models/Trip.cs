using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

// Định nghĩa model Trip
public class Trip
{
    // BsonId để ánh xạ với trường _id của MongoDB
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string TripNumber { get; set; }
    public string Status { get; set; }
    public string StartLocation { get; set; }
    public string EndLocation { get; set; }

    // Dùng để ánh xạ với trường driver_id trong MongoDB
    [BsonRepresentation(BsonType.ObjectId)]
    public string? DriverId { get; set; }

    public DateTime EstimatedCompletionTime { get; set; }
    public DateTime? ActualCompletionTime { get; set; }

    // Trường này chỉ dùng trong aggregate lookup, không có trong DB
    [BsonIgnore]
    public List<Driver> DriverDetails { get; set; }
}