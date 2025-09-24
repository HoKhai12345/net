using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using TransportApi.Models;
using TransportApi.Interface;

namespace TransportApi.Services
{
    public class RoleSeederHostedService : IHostedService
    {
        private readonly IMongoDbService _mongo;

        public RoleSeederHostedService(IMongoDbService mongo)
        {
            _mongo = mongo;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine(">>> RoleSeeder chạy...");
            var memberRole = await _mongo.Roles.Find(r => r.Name == "Member").FirstOrDefaultAsync(cancellationToken);
            // Ensure Member role exists with no permissions by default
            if (memberRole == null)
            {
                var role = new Role
                {
                    Name = "Member",
                    PermissionIds = new System.Collections.Generic.List<string>()
                };
                await _mongo.Roles.InsertOneAsync(role, cancellationToken: cancellationToken);
            }

            // Seed admin
            var adminRole = await _mongo.Roles.Find(r => r.Name == "Admin").FirstOrDefaultAsync(cancellationToken);
            if (adminRole == null)
            {
                var role = new Role
                {
                    Name = "Admin",
                    PermissionIds = new System.Collections.Generic.List<string>()
                };
                await _mongo.Roles.InsertOneAsync(role, cancellationToken: cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}


