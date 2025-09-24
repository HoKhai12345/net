using MongoDB.Driver;
using TransportApi.Models;

namespace TransportApi.Interface
{
    public interface IMongoDbService
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<Role> Roles { get; }
        IMongoCollection<Permission> Permissions { get; }
    }
}
