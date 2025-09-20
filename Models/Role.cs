public class Role
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }

    public List<string> Permissions { get; set; } = new List<string>();
}