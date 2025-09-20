using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? name { get; set; }
    public string? username { get; set; }
    public string? password { get; set; }
    public DateTime EstimatedCompletionTime { get; set; }
    public DateTime? ActualCompletionTime { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}