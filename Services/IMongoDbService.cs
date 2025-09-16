// MongoDbService.cs
using MongoDB.Driver;
using Microsoft.Extensions.Options;

public interface IMongoDbService
{
    IMongoCollection<T> GetCollection<T>(string name);
}

public class MongoDbService : IMongoDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        _database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}
