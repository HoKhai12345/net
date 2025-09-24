using MongoDB.Driver;
using TransportApi.Models;
using Microsoft.Extensions.Configuration;
using TransportApi.Interface;

namespace TransportApi.Services
{
    public class MongoDbService : IMongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            var mongoDbSettings = configuration.GetSection("MongoDbSettings");
            var client = new MongoClient(mongoDbSettings.GetValue<string>("ConnectionString"));
            _database = client.GetDatabase(mongoDbSettings.GetValue<string>("DatabaseName"));
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
        public IMongoCollection<Role> Roles => _database.GetCollection<Role>("roles");
        public IMongoCollection<Permission> Permissions => _database.GetCollection<Permission>("permissions");
    }
}
