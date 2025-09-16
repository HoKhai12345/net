using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TransportApi.Models;

namespace TransportApi.Services
{
    public class VehicleService
    {
        private readonly IMongoCollection<Vehicle> _vehicles;

        public VehicleService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _vehicles = database.GetCollection<Vehicle>("Vehicles");
        }

        // Lấy toàn bộ
        public async Task<List<Vehicle>> GetAsync() =>
            await _vehicles.Find(_ => true).ToListAsync();

        // Lấy theo Id
        public async Task<Vehicle?> GetAsync(string id) =>
            await _vehicles.Find(v => v.Id == id).FirstOrDefaultAsync();

        // Tạo mới
        public async Task CreateAsync(Vehicle vehicle) =>
            await _vehicles.InsertOneAsync(vehicle);
    }

    // class dùng để map config từ appsettings.json
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}
