public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    public MongoDbContext(IConfiguration configuration)
    {
        var mongoDbSettings = configuration.getSection("MongoDbSettings");
        var client = new MongoClient(mongoDbSettings.GetValue<string>("ConnectionString"));
        _database = client.GetDatabase(mongoDbSettings.GetValue<string>("DatabaseName"));
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("users");
    public IMongoCollection<Role> Roles => _database.GetCollection<Role>("roles");
}