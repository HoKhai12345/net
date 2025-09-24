using TransportApi.Models;
using TransportApi.Dto;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using static System.Text.Json.JsonSerializer;
using TransportApi.Interface;

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

        public async Task<Role> AddRole(RoleDto roleDto)
        {
            Console.WriteLine("NAME", roleDto.name);
            var existingRole = await _mongoDbService.Roles.Find(r => r.Name == roleDto.name).FirstOrDefaultAsync();
            if (existingRole != null)
            {
                return null; // Role already exists
            }
            var role = new Role
            {
                Name = roleDto.name
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

        public async Task<Role> UpdateRole(RoleDto roleDto, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var existingRole = await _mongoDbService.Roles
                .Find(r => r.Id == id)
                .FirstOrDefaultAsync();
            Console.WriteLine("existingRole: " + Serialize(existingRole));
            if (existingRole == null)
            {
                Console.WriteLine("Received registration request for user: " + Serialize(roleDto));
                return null;
            }

            existingRole.Name = roleDto.name;
            var filter = Builders<Role>.Filter.Eq(r => r.Id, id);

            // Tạo câu lệnh cập nhật.
            // UpdateDefinition là cách an toàn để cập nhật nhiều trường.
            var update = Builders<Role>.Update
                .Set(r => r.Name, existingRole.Name);
            // Thực thi cập nhật và trả về object đã được cập nhật.
            await _mongoDbService.Roles.UpdateOneAsync(filter, update);
            return existingRole;
        }
    }
}