using TransportApi.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;

namespace TransportApi.Services
{
    public class RolesService : IRoleService
    {
        private readonly IMongoDbService _mongoDbService;
        private readonly IConfiguration _configuration;

        public RolesService(IMongoDbService mongoDbService, IConfiguration configuration)
        {
            _mongoDbService = mongoDbService;
            _configuration = configuration;
        }

        public async Task<Role> AddRole(string name)
        {
            var existingRole = await _mongoDbService.Roles.Find(r => r.Name == name).FirstOrDefaultAsync();
            if (existingRole != null)
            {
                return null; // Role already exists
            }
            var role = new Role
            {
                Name = name
            };
            await _mongoDbService.Roles.InsertOneAsync(role);
            return role;
        }

        public async Task<List<Role>> ListRole(int limit)
        {
            // Check if limit is a valid number to prevent errors
            if (limit <= 0)
            {
                return new List<Role>(); // Return an empty list or throw an exception
            }

            // Use .Limit() to specify the number of documents to retrieve
            var roles = await _mongoDbService.Roles.Find(_ => true).Limit(limit).ToListAsync();

            return roles;
        }
    }
}